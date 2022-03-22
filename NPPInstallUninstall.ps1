#cmdlets to get the filename and version of the notepad++ installer in remote folder
$RemoteInstallerFileLocation = "C:\npp"
$SystemArchitecture = If((gwmi win32_operatingsystem | select osarchitecture).osarchitecture -eq "64-bit") {"64"} else {"32"}
$NPPInstallerFN = (Get-ChildItem $RemoteInstallerFileLocation -File -recurse | Where{$_.Name -match $SystemArchitecture}).Name
$RemoteNPPVersion = $NPPInstallerFN.Substring(4,$NPPInstallerFN.IndexOf(".Installer")-4)
$RemoteNPPVersionArr = $RemoteNPPVersion -Split "\."

#cmdlet to get the installed notepad++ pacakage
$NPP=Get-Package -Provider Programs -IncludeWindowsInstaller -Name "Notepad++*"
$currenNPPVersion = $NPP.Version
$CurrentNPPVersionArr = $currenNPPVersion -Split "\."

$ContinueInstalltion = $false

$LoopVar = 0

if($RemoteNPPVersionArr.length -gt $CurrentNPPVersionArr.length)
{
	for ($i = 1; $i -le $RemoteNPPVersionArr.length-$CurrentNPPVersionArr.length; $i++)
	{
		$currenNPPVersion = $currenNPPVersion + ".0"
	}
	$CurrentNPPVersionArr = $currenNPPVersion -Split "\."
	
}elseif($CurrentNPPVersionArr.length -gt $RemoteNPPVersionArr.length)
{
	for ($i = 1; $i -le $CurrentNPPVersionArr.length-$RemoteNPPVersionArr.length; $i++)
	{
		$RemoteNPPVersion = $RemoteNPPVersion + ".0"
	}
	$RemoteNPPVersionArr = $RemoteNPPVersion -Split "\."
}


#Current Version and remote version comparision
foreach($Version in $RemoteNPPVersionArr)
{
	if ([int]$Version -gt [int]$CurrentNPPVersionArr[$LoopVar])
	{
		$ContinueInstallation = $true
		break
	}
	
}


if($ContinueInstallation)
{
	(Get-Process -Name "*Notepad++*").Kill()
	Get-Package *Notepad++*|Uninstall-Package
	$LocalInstallerFileLocation = "C:\"
	New-Item -Path $LocalInstallerFileLocation -Name "nppinstaller" -ItemType "directory" -Force
	$LocalNPPInstallerFilePath = $LocalInstallerFileLocation+"nppinstaller"+$NPPInstallerFN
	Copy-Item -Path $RemoteInstallerFileLocation+$NPPInstallerFN -Destination $LocalNPPInstallerFilePath
	get-package *Notepad++* | % { & ($_.metadata['uninstallstring'] -replace '"') /S }
	Start-Process $LocalNPPInstallerFilePath "/S"
	Remove-Item $LocalInstallerFileLocation+"nppinstaller" -Recurse
}




