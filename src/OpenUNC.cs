using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;

class OpenUNC
{
    static void Main( string[] args )
    {
        string inputPath = string.Join( " ", args );
        try
        {
            Uri uri = new Uri( Uri.UnescapeDataString( string.Join( "%20", args ).Replace( "#", "%23" ) ) );
            inputPath = uri.LocalPath + Uri.UnescapeDataString( uri.Fragment );
            CheckAccessible( uri );
            Process.Start( GetOpenPath( inputPath ) );
        }
        catch( Exception e )
        {
            MessageBox.Show( String.Format( "{0}\n\"{1}\"", e.Message, inputPath ),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
        }
    }

    private static void CheckAccessible( Uri uri )
    {
        if( uri.IsUnc )
        {
            if( new Ping().Send( uri.Host, 100 ).Status != IPStatus.Success )
            {
                throw new Exception( String.Format( "Network \"{0}\" not accessible.", uri.Host ) );
            }
        }
        else if( uri.IsLoopback )
        {
            DriveInfo drive = new DriveInfo( Path.GetPathRoot( uri.LocalPath ) );
            if( drive.DriveType != DriveType.Network )
            {
                if( !drive.IsReady )
                {
                    throw new Exception( String.Format( "Drive \"{0}\" not accessible.", drive.Name ) );
                }
            }
        }
    }

    private static string GetOpenPath( string inputPath )
    {
        string openPath = inputPath;
        while( !File.Exists( openPath ) && !Directory.Exists( openPath ) )
        {
            DirectoryInfo directoryInfo = Directory.GetParent( openPath );
            if( directoryInfo != null )
            {
                openPath = directoryInfo.FullName;
            }
            else
            {
                throw new Exception( String.Format( "Not found." ) );
            }
        }
        if( openPath.Length != inputPath.Length )
        {
            MessageBox.Show( String.Format( "Open:\n\"{0}\"\n\nNot found.\n\"{1}\"", openPath, inputPath ),
                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }

        return openPath;
    }
}