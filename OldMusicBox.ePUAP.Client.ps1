# /bin/release
$dll462  = Resolve-Path "./OldMusicBox.ePUAP.Client/bin/Release/OldMusicBox.ePUAP.Client.dll"
$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($dll462).FileVersion

# utwórz folder
$path = ("./OldMusicBox.ePUAP.Client/bin/" + $version)

New-Item -ItemType Directory -Force -Path $path
New-Item -ItemType Directory -Force -Path ($path + "/lib")
New-Item -ItemType Directory -Force -Path ($path + "/lib/net462")

Copy-Item $dll462 ($path + "/lib/net462")

# nuspec
$nuspec = ($path + "/OldMusicBox.ePUAP.Client.nuspec")
$contents = @"
<?xml version="1.0"?>
<package>
  <metadata>
    <id>OldMusicBox.ePUAP.Client</id>
    <version>__VERSION</version>
    <authors>wzychla</authors>
	<dependencies>
		<group targetFramework="net462">
		</group>
	</dependencies>
    <projectUrl>https://github.com/wzychla/OldMusicBox.ePUAP.Client</projectUrl>
	<repository type="git" url="https://github.com/wzychla/OldMusicBox.ePUAP.Client.git" />
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
	<license type="expression">AGPL-3.0-only</license>
    <description>OldMusicBox.ePUAP.Client. Independent ePUAP Client implementation.</description>
    <copyright>Copyright 2020 Wiktor Zychla</copyright>
	<icon>images\icon.png</icon>
    <tags>SAML2</tags>
  </metadata>
  <files>
	<file src="lib/net462/OldMusicBOx.ePUAP.Client.dll" target="lib/net462/OldMusicBox.ePUAP.Client.dll" />  
	<file src="..\..\..\icon.png" target="images\" />
  </files>
</package>
"@

$contents = $contents -replace "__VERSION", $version

New-Item -ItemType File -Path $nuspec -value $contents

# spakuj
Set-Location -Path $path
nuget pack

Set-Location -Path "../../.."