# Dùng: .\doi-mssv.ps1 -Mssv BIT240123
# Đổi toàn bộ "BIT240009" (MSSV mẫu) thành MSSV của bạn: nội dung file + tên file.
param([Parameter(Mandatory = $true)][string]$Mssv)

$old = 'BIT240009'
$utf8Bom = New-Object System.Text.UTF8Encoding($true)

Get-ChildItem -Path . -Recurse -File -Include *.cs, *.cshtml, *.json, *.csproj |
    Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' } |
    ForEach-Object {
        $text = [IO.File]::ReadAllText($_.FullName)
        if ($text.Contains($old)) {
            [IO.File]::WriteAllText($_.FullName, $text.Replace($old, $Mssv), $utf8Bom)
        }
    }

Get-ChildItem -Path . -Recurse -File |
    Where-Object { $_.Name -like "*$old*" -and $_.FullName -notmatch '[\\/](bin|obj)[\\/]' } |
    ForEach-Object { Rename-Item -LiteralPath $_.FullName -NewName $_.Name.Replace($old, $Mssv) }

Write-Host "Da doi MSSV thanh $Mssv. Database moi: ${Mssv}_MID"
