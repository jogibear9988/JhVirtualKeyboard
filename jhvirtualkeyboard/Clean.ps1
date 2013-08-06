function Get-ScriptDirectory
{
	$Invocation = (Get-Variable MyInvocation -Scope 1).Value
	Split-Path $Invocation.MyCommand.Path
}
$thisDir = Get-ScriptDirectory
Write-Host 'Deleting all build-product files from JhVirtualKeyboartd..'
@(
	$thisDir + '\bin'
	$thisDir + '\obj'
	$thisDir + '\JhVirtualKeyboardDemoApp\bin'
	$thisDir + '\JhVirtualKeyboardDemoApp\obj'
) |
Where-Object { Test-Path $_ } |
ForEach-Object { Remove-Item $_ -Recurse -Force -ErrorAction Stop }

