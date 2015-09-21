package perfit.util.SerFile;


import java.io.BufferedWriter;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;

public class SFile {
	private String _path;
	private DataInputStream _dataInputStream = null;
	private DataOutputStream _dataOutputStream = null;
	private long _fileSize;
	public SFile(String path)
	{
		_path = path;
		File file = new File(_path);
		_fileSize = file.length();
	}

	public byte[] ReadFileBinary() throws IOException
	{
		int size = 1000;
		byte buf[] = new byte[0];
		if (_dataInputStream == null)
		{
			_dataInputStream = new DataInputStream(new FileInputStream(_path));
		}
		
		if (_fileSize == 0)
		{
			return buf;
		}
		if (_fileSize > size)
		{
			_fileSize -= size;
		}
		else
		{
			size = (int) _fileSize;
			_fileSize = 0;
		}
		buf = new byte[size];
		_dataInputStream.read(buf);
		return buf;
	}
	
	public void WriteFileBinary(byte buf[]) throws IOException
	{
		if (_dataOutputStream == null)
		{
			_dataOutputStream = new DataOutputStream(new FileOutputStream(_path));
		}
		_dataOutputStream.write(buf);
		_dataOutputStream.flush();
	}

	public void WriteLine(String buf)
	{
		try
		{
			FileWriter fileWriter = new FileWriter(_path, true);
			BufferedWriter bufferedWriter = new BufferedWriter(fileWriter);
			buf += "\r\n";
			bufferedWriter.write(buf);
			bufferedWriter.flush();
			bufferedWriter.close();
			fileWriter.close();
		}
		catch (IOException e)
		{
			e.printStackTrace();
		}
	}
	
}
