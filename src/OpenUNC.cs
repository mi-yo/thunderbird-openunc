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

            string targetPath = TraverseValidPath( inputPath );
            if( targetPath.Length != inputPath.Length )
            {
                MessageBox.Show( String.Format( "Open:\n\"{0}\"", targetPath ),
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }

            if( Directory.Exists( targetPath ) )
            {
                Process.Start( targetPath );
            }
            else
            {
                Process.Start( "explorer.exe", String.Format( "/select, \"{0}\"", targetPath ) );
            }
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
            PingReply pingReply = null;
            try
            {
                pingReply = new Ping().Send( uri.Host, 100 );
            }
            finally
            {
                if( pingReply == null || pingReply.Status != IPStatus.Success )
                {
                    throw new Exception( String.Format( "Network \"{0}\" not accessible.", uri.Host ) );
                }
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

    private static string TraverseValidPath( string inputPath )
    {
        string targetPath = inputPath;
        while( !File.Exists( targetPath ) && !Directory.Exists( targetPath ) )
        {
            DirectoryInfo directoryInfo = Directory.GetParent( targetPath );
            if( directoryInfo != null )
            {
                targetPath = directoryInfo.FullName;
            }
            else
            {
                throw new Exception( String.Format( "Not found." ) );
            }
        }

        return targetPath;
    }
}
