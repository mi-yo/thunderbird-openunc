thunderbird-openunc
===================

Open multibyte UNC links in Mozilla Thunderbird via external program under Windows.

Why is it necessary?
-------------------

Windows explorer does not open multibyte UNC links in Thunderbird.  
`file:///\\servername\Ｍｕｌｔｉｂｙｔｅ`  
Since opened with url encoded, decode via external program.


Settings
-------------------

1. Build C# program to `OpenUNC.exe` by `build.bat`. (need .NET Framework)
2. Open UNC links via `OpenUNC.exe` in Thunderbird. (also need .NET Framework)  
   https://support.mozilla.org/kb/configuration-options-attachments


Tips
-------------------

- Thunderbird does not encode space ` ` to `%20`.  
  If UNC path includes space, need to convert manually.  
  `file:///\\servername\path to\file` -> `file:///\\servername\path%20to\file`

- [URL Link](https://addons.mozilla.org/thunderbird/addon/url-link/) addon is useful with this program.
