<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:c="Crap4n">
  <xsl:output method="html"/>
  <xsl:variable name="report.root" select="//c:crapResult" />
  <xsl:variable name="crapThreshold" select="$report.root/c:summary/@crapThreshold"/>
  <xsl:template match="/">
    <xsl:variable name="report.root" select="//c:crapResult" />
    <table class="section-table" cellpadding="2" cellspacing="0" border="0" width="98%">
      <tr>
        <td class="sectionheader" colspan="2">
          <xsl:variable name="totalMethods" select="$report.root/c:summary/@totalMethods"/>
          <xsl:variable name="crappyMethods" select="$report.root/c:summary/@crappyMethods"/>
          <xsl:variable name="crapLoad" select="$report.root/c:summary/@crapLoad"/>
          <xsl:variable name="percent" select="round(1000 * $crappyMethods div $totalMethods) div 10" />
          <xsl:value-of select="concat ($percent, '% of the methods are crappy (',$crappyMethods, ' of ', $totalMethods, ').')" />
          <br />
          <xsl:value-of select="concat ('Total crap load is ', $crapLoad)" />
        </td>
      </tr>
      <tr>
        <td colspan="2"/>
      </tr>
      <!-- -->
      <tr>
        <td>
          <table class="section-table" cellpadding="2" cellspacing="0" border="0" width="98%" bgColor="silver">
            <tr>
              <th align="Left">crap</th>
              <th align="Left">crap load</th>
              <th align="Left">Cyclomatic Complexity</th>
              <th align="Left">Code Coverage</th>
              <th align="Left">method</th>
              <th align="Left">source file</th>
            </tr>
            <xsl:apply-templates select="$report.root/c:methods/c:method[c:crap>$crapThreshold]">
              <xsl:sort select="c:crap" data-type="number" order="descending" />
              <xsl:sort select="c:codeCoverage" data-type="number" order="descending" />
            </xsl:apply-templates>
          </table>
        </td>
      </tr>
      <!-- -->
    </table>
  </xsl:template>
  <!-- -->
  <xsl:template name="render" match="c:method">
        <tr bgcolor="Gainsboro">
          <td align="right">
            <xsl:value-of select="c:crap"/>
          </td>
          <td align="right">
            <xsl:value-of select="c:crapLoad"/>
          </td>
          <td align="right">
            <xsl:value-of select="c:cyclomaticComplexity"/>
          </td>
          <td align="right">
            <xsl:value-of select="c:codeCoverage"/>
          </td>
          <td align="top">
            <xsl:value-of select="@namespace"/>.<xsl:value-of select="@class"/>.<xsl:value-of select="@name"/>
          </td>
          <td align="top">
            <xsl:value-of select="c:sourceFile/text()"/><br />line <xsl:value-of select="c:sourceFile/@lineNumber"/>.
          </td>
        </tr>
  </xsl:template>
</xsl:stylesheet>