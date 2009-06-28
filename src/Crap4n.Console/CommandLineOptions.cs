﻿// File: CommandLineOptions.cs
//
// This is a re-usable component to be used when you 
// need to parse command-line options/parameters.
//
// Separates command line parameters from command line options.
// Uses reflection to populate member variables the derived class with the values 
// of the options.
//
// An option can start with "/", "-" or "--".
//
// I define 3 types of "options":
//   1. Boolean options (yes/no values), e.g: /r to recurse
//   2. Value options, e.g: /loglevel=3
//   2. Parameters: standalone strings like file names
//
// An example to explain:
//   csc /nologo /t:exe myfile.cs
//       |       |      |
//       |       |      + parameter
//       |       |
//       |       + value option
//       |
//       + boolean option
//
// Please see a short description of the CommandLineOptions class
// at http://codeblast.com/~gert/dotnet/sells.html
// 
// Gert Lombard (gert@codeblast.com)
// James Newkirk (jim@nunit.org)

namespace Codeblast
{
    using System;
    using System.Reflection;
    using System.Collections;
    using System.Text;

    //
    // The Attributes
    //

    [AttributeUsage(AttributeTargets.Field)]
    public class OptionAttribute : Attribute
    {
        protected object _optValue;
        protected string _optName;
        protected string _description;

        public string Short
        {
            get { return _optName; }
            set { _optName = value; }
        }

        public object Value
        {
            get { return _optValue; }
            set { _optValue = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }

    //
    // The CommandLineOptions members
    //

    public abstract class CommandLineOptions
    {
        protected ArrayList _parameters;
        private readonly int _optionCount;

        protected CommandLineOptions(string[] args)
        {
            _optionCount = Init(args);
        }

        public bool NoArgs
        {
            get
            {
                return ParameterCount == 0 && _optionCount == 0;
            }
        }

        public int Init(string[] args)
        {
            int count = 0;
            int n = 0;
            while (n < args.Length)
            {
                int pos = IsOption(args[n]);
                if (pos > 0)
                {
                    // It's an option:
                    if (GetOption(args, ref n, pos))
                        count++;
                    else
                        InvalidOption(args[Math.Min(n, args.Length - 1)]);
                }
                else
                {
                    // It's a parameter:
                    if (_parameters == null) _parameters = new ArrayList();
                    _parameters.Add(args[n]);
                }
                n++;
            }
            return count;
        }

        // An option starts with "/", "-" or "--":
        protected virtual int IsOption(string opt)
        {
            char[] c;
            if (opt.Length < 2)
            {
                return 0;
            }
            if (opt.Length > 2)
            {
                c = opt.ToCharArray(0, 3);
                if (c[0] == '-' && c[1] == '-' && IsOptionNameChar(c[2]))
                    return 2;
            }
            else
            {
                c = opt.ToCharArray(0, 2);
            }
            if ((c[0] == '-' || c[0] == '/') && IsOptionNameChar(c[1]))
                return 1;
            return 0;
        }

        protected virtual bool IsOptionNameChar(char c)
        {
            return Char.IsLetterOrDigit(c) || c == '?';
        }

        protected abstract void InvalidOption(string name);

        protected virtual bool MatchShortName(FieldInfo field, string name)
        {
            object[] atts = field.GetCustomAttributes(typeof(OptionAttribute), true);
            foreach (OptionAttribute att in atts)
            {
                if (string.Compare(att.Short, name, true) == 0) return true;
            }
            return false;
        }

        protected virtual FieldInfo GetMemberField(string name)
        {
            Type t = GetType();
            FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                if (string.Compare(field.Name, name, true) == 0) return field;
                if (MatchShortName(field, name)) return field;
            }
            return null;
        }

        protected virtual object GetOptionValue(FieldInfo field)
        {
            object[] atts = field.GetCustomAttributes(typeof(OptionAttribute), true);
            if (atts.Length > 0)
            {
                var att = (OptionAttribute)atts[0];
                return att.Value;
            }
            return null;
        }

        protected virtual bool GetOption(string[] args, ref int index, int pos)
        {
            try
            {
                object cmdLineVal = null;
                string opt = args[index].Substring(pos, args[index].Length - pos);
                SplitOptionAndValue(ref opt, ref cmdLineVal);
                FieldInfo field = GetMemberField(opt);
                if (field != null)
                {
                    object value = GetOptionValue(field);
                    if (value == null)
                    {
                        if (field.FieldType == typeof(bool))
                            value = true; // default for bool values is true
                        else if (field.FieldType == typeof(string))
                        {
                            value = cmdLineVal ?? args[++index];
                            field.SetValue(this, Convert.ChangeType(value, field.FieldType));
                            var stringValue = (string)value;
                            if (string.IsNullOrEmpty(stringValue))
                                return false;
                            return true;
                        }
                        else
                            value = cmdLineVal ?? args[++index];
                    }
                    field.SetValue(this, Convert.ChangeType(value, field.FieldType));
                    return true;
                }
            }
            catch (Exception)
            {
                // Ignore exceptions like type conversion errors.
            }
            return false;
        }

        protected virtual void SplitOptionAndValue(ref string opt, ref object val)
        {
            // Look for ":" or "=" separator in the option:
            int pos = opt.IndexOfAny(new[] { ':', '=' });
            if (pos < 1) return;

            val = opt.Substring(pos + 1);
            opt = opt.Substring(0, pos);
        }

        // Parameter accessor:
        public string this[int index]
        {
            get
            {
                if (_parameters != null) return (string)_parameters[index];
                return null;
            }
        }

        public ArrayList Parameters
        {
            get { return _parameters; }
        }

        public int ParameterCount
        {
            get
            {
                return _parameters == null ? 0 : _parameters.Count;
            }
        }

        public virtual void Help()
        {
            Console.WriteLine(GetHelpText());
        }

        public virtual string GetHelpText()
        {
            var helpText = new StringBuilder();

            Type t = GetType();
            FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                object[] atts = field.GetCustomAttributes(typeof(OptionAttribute), true);
                if (atts.Length > 0)
                {
                    var att = (OptionAttribute)atts[0];
                    if (att.Description != null)
                    {
                        string valType = "";
                        if (att.Value == null)
                        {
                            if (field.FieldType == typeof(float)) valType = "=FLOAT";
                            else if (field.FieldType == typeof(string)) valType = "=STR";
                            else if (field.FieldType != typeof(bool)) valType = "=X";
                        }

                        helpText.AppendFormat("/{0,-20}{1}", field.Name + valType, att.Description);
                        if (att.Short != null)
                            helpText.AppendFormat(" (Short format: /{0}{1})", att.Short, valType);
                        helpText.Append(Environment.NewLine);
                    }
                }
            }
            return helpText.ToString();
        }
    }
}
