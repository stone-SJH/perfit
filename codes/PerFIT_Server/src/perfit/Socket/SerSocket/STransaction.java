package perfit.Socket.SerSocket;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.io.UnsupportedEncodingException;
import java.net.Socket;
import java.nio.channels.FileChannel;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;






import java.util.Map;

import perfit.UserOperation.SUserOperation;
import perfit.consistency.GetFile;
import perfit.statistic.service.DataHandler;
import perfit.statistic.service.PushupDataHandler;
import perfit.statistic.service.ShoulderDataHandler;
import perfit.util.SerFile.SFile;
import perfit.util.SerFile.SFileLocation;
import perfit.util.SerFile.SSystemLog;
import perfit.util.SerFile.SUserLog;

public class STransaction extends Thread{
	private Socket _socket;
	private BufferedReader _bufferedReader;
	private InputStream _inputStream;
	private OutputStream _outputStream;
	private PrintWriter _printWriter;
	private SUserOperation _user;
	private Map<Long, String> _threadManage;
	private long _threadId;
	private static String _split = "/";
	public STransaction(Socket socket, List <String> userList, Map<Long, String> threadManage) throws IOException
	{
		new SSystemLog().WriteInfo("The connection " + socket.getInetAddress() + ": "
									+ socket.getPort() + " is opened.");
		_threadManage = threadManage;
		_socket = socket;
		_inputStream = _socket.getInputStream();
		_outputStream = socket.getOutputStream();
		_bufferedReader = new BufferedReader(new InputStreamReader(_inputStream));
		_printWriter = new PrintWriter(new OutputStreamWriter(_outputStream), true);
		_user = new SUserOperation();
		
	}
	
	public void run()
	{
		_threadManage.replace((long) 0, "true");
		_threadId = getId();
		_threadManage.put(_threadId, "run");
		while (true)
		{
			if (_threadManage.get(_threadId) == "run")
			{
				String recvMessage = "";
				try {
					recvMessage = RecvString();
				} catch (IOException e) {
					break;
				}
				if (recvMessage == "")
				{
					
					break;
				}
				if (recvMessage == "exit")
				{
					break;
				}
				String[] message = recvMessage.split(" ");
				if (message[0].equals("closeconnection"))
				{
					if (_user.GetUser() != null)
					{
						_user.RemoveUser(_user.GetUser());
					}
					break;
				}
				HandleMessage(message);
			}
			if (_threadManage.get(_threadId) == "exit")
			{
				break;
			}
		}
		_threadManage.remove(_threadId);
		_threadManage.replace((long) 0, "true");
		try {
			_socket.close();
			if (_user.GetUser() != null)
			{
				_user.RemoveUser(_user.GetUser());
			}
		} catch (IOException e) {
			if (_user.GetUser() != null)
			{
				_user.RemoveUser(_user.GetUser());
			}
			new SSystemLog().WriteError(e.toString());
		}
		
		if (_user.GetUser() != null)
		{
			new SUserLog(_user.GetUser().getUsername()).WriteInfo(_user.GetUser().getUsername() + ": log out at " 
							+ _socket.getInetAddress() + ": " + _socket.getPort() + ".");
		}
		new SSystemLog().WriteInfo("The connection " + _socket.getInetAddress() 
									+ ": " + _socket.getPort() + " is closed.");		
		
	}


	private void SendString(String message)
	{
		_printWriter.print(message);
		_printWriter.flush();
	}

	private String RecvString() throws IOException
	{
		String ret = "";
		String len = "";
		int length;
		char ch;
		try {
			 if ((ch = (char) _bufferedReader.read())  ==  -1)
             {
                     return  "";
             }
             len += ch;
			while ((ch = (char) _bufferedReader.read()) != ' ')
			{
				len += ch; 
			}
			length = Integer.valueOf(len).intValue();
			
			char buf[] = new char[length];
			_bufferedReader.read(buf, 0, length);
			ret = String.valueOf(buf);
		} catch (IOException e) {
			new SSystemLog().WriteError(e.toString());
			ret = "";
			throw e;
		}	
		return ret;
	}
	
	private byte[] RecvByte(int length)
	{
		byte [] ret = new byte[length];
		try {
			_inputStream.read(ret);
		} catch (IOException e) {
			new SSystemLog().WriteError(e.toString());
		}
		return ret;
	}
	
	private void SendByte(byte[] buf)
	{ 
		try {
			_outputStream.write(buf);
			_outputStream.flush();
		} catch (IOException e) {
			new SSystemLog().WriteError(e.toString());
		}
	}
	private void HandleMessage(String[]message)
	{
		switch (message[0])
		{
			case "login":{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				_threadManage.replace(_threadId, "login");
				_threadManage.replace((long) 0, "true");
				String username = message[1];
				String password = message[2];
				if (_user.Login(username, password) == true)
				{
					new SUserLog(username).WriteInfo(username + ": log in at " 
														+ _socket.getInetAddress() + ": " 
														+ _socket.getPort() + ".");
					SendString("success");
				}
				else
				{
					SendString("failure");
				}	
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
			case "register":{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				_threadManage.replace(_threadId, "register");
				_threadManage.replace((long) 0, "true");
				String username = message[1];
				String password = message[2];
				if (_user.Register(username, password) == true)
				{
					new SSystemLog().WriteInfo("The user " + username + " is registered.");
					try {
						Mkdir(_user.GetUser().getUsername());
					} catch (IOException e) {
						
						new SSystemLog().WriteError(e.toString());
					}
					SendString("success");
				}
				else
				{
					SendString("failure");
				}	
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
			case "modifyinfo":{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				
				_threadManage.replace(_threadId, "modifyinfo");
				_threadManage.replace((long) 0, "true");
				if (_user.GetUser() == null)
				{
					SendString("failure");
					_threadManage.replace(_threadId, "run");
					break;
				}
				String username = message[1];
				String hei = message[2];
				String wei = message[3];
				double height = Double.parseDouble(hei);
				double weight = Double.parseDouble(wei);
				if (_user.ModifyInfo(username, height, weight))
				{
					new SSystemLog().WriteInfo("The information of the user " + _user.GetUser().getUsername() + " is modified.");
					SendString("success");
				}
				else
				{					
					SendString("failure");
				}
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
			case "modifypass":{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				
				_threadManage.replace(_threadId, "modifypass");
				_threadManage.replace((long) 0, "true");
				if (_user.GetUser() == null)
				{
					SendString("failure");
					_threadManage.replace(_threadId, "run");
					break;
				}
				String username = message[1];
				String password = message[2];
				if (_user.ModifyPass(username, password))
				{
					new SSystemLog().WriteInfo("The password of the user " + _user.GetUser().getUsername() + " is modified.");
					SendString("success");
				}
				else
				{
					SendString("failure");
				}
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
			case "sendfile":{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				_threadManage.replace(_threadId, "sendfile");
				_threadManage.replace((long) 0, "true");
				if (_user.GetUser() == null)
				{
					SendString("failure");
					_threadManage.replace(_threadId, "run");
					break;
				}
				String filename = message[1];
				int size = Integer.valueOf(message[2]).intValue();
				int lastSize = Integer.valueOf(message[3]).intValue();
				String folder = SFileLocation.UserFolder.GetPath() + _user.GetUser().getUsername();  
				String path = folder + _split + filename;
				SendString("success");
				SFile sfile = new SFile(path);
				for (int i = 0; i < size; i++)
				{
					int length = 1000;
					if (i == size - 1)
					{
						length = lastSize;
					}
					try {
						sfile.WriteFileBinary(RecvByte(length));
					} catch (IOException e) {
						new SUserLog(_user.GetUser().getUsername()).WriteError(e.toString());
						SendString("failure");
						_threadManage.replace(_threadId,  "run");
						_threadManage.replace((long) 0, "true");
						break;
					}
					SendString("success");
				}
				sfile = null;
				System.gc();
				new SUserLog(_user.GetUser().getUsername()).WriteInfo( _user.GetUser().getUsername() + ": send the file " + filename);
				SendString("success");
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
			case "recvfile":{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				_threadManage.replace(_threadId, "recvfile");
				_threadManage.replace((long) 0, "true");
				if (_user.GetUser() == null)
				{
					SendString("failure");
					_threadManage.replace(_threadId, "run");
					_threadManage.replace((long) 0, "true");
					break;
				}
				String filename = message[1];
				String folder = SFileLocation.UserFolder.GetPath() + _user.GetUser().getUsername();
						
				String path = folder + _split + filename;
				SFile sfile = new SFile(path);
				List<byte[]> content = new ArrayList<byte[]>();
				byte[] buf = null;
				int lastSize = 0;
				try {
					while((buf = sfile.ReadFileBinary()).length != 0 )
					{
						content.add(buf);
						lastSize = buf.length;
					}
				} catch (IOException e) {
					// TODO Auto-generated catch block
					new SUserLog(_user.GetUser().getUsername()).WriteError(e.toString()); 
					SendString("failure");
					_threadManage.replace(_threadId,  "run");
					_threadManage.replace((long) 0, "true");
					break;
				}
				int size = content.size();
				String mess = String.valueOf(size) + ' ' + String.valueOf(lastSize);
				
				SendString(mess);
				for (int i = 0; i < size; i++)
				{
					String recvMessage = "";
					try {
						recvMessage = RecvString();
					} catch (IOException e) {
						
					}
					if (!recvMessage.equals("success"))
					{
						_threadManage.replace(_threadId, "run");
						_threadManage.replace((long) 0, "true");
						break;
					}
					SendByte(content.get(i));
				}
				new SUserLog(_user.GetUser().getUsername()).WriteInfo( _user.GetUser().getUsername() + ": receive the file " + filename);
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
			case "checkfile":{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				_threadManage.replace(_threadId, "checkfile");
				_threadManage.replace((long) 0, "true");
				if (_user.GetUser() == null)
				{
					SendString("failure");
					_threadManage.replace(_threadId, "run");
					_threadManage.replace((long) 0, "true");
					break;
				}
				String folder;
				folder = SFileLocation.UserFolder.GetPath() + _user.GetUser().getUsername(); 
				List <String> content =  new GetFile().getFile(folder);
				List <byte[]> cont = new ArrayList <byte[]>();
				int size = content.size();
				String mess = String.valueOf(size); 
				for (int i = 0; i < size; i++)
				{
					try {
						cont.add(content.get(i).getBytes("UTF-8"));
					} catch (UnsupportedEncodingException e) {
						// TODO Auto-generated catch block
						new SUserLog(_user.GetUser().getUsername()).WriteError(e.toString()); 
						SendString("failure");
						_threadManage.replace(_threadId,  "run");
						_threadManage.replace((long) 0, "true");
						break;
					}
					mess += ' ' + String.valueOf(cont.get(i).length);
				}
				SendString(mess);
				try {
					RecvString();
				} catch (IOException e1) {
					
				}
				for (int i = 0; i < size; i++)
				{
					try {
						SendByte(content.get(i).getBytes("UTF-8"));
					} catch (UnsupportedEncodingException e) {
						// TODO Auto-generated catch block
						new SUserLog(_user.GetUser().getUsername()).WriteError(e.toString()); 
						SendString("failure");
						_threadManage.replace(_threadId,  "run");
						_threadManage.replace((long) 0, "true");
						break;
					}
					try {
						RecvString();
					} catch (IOException e) {
						
					}
				}
				new SUserLog(_user.GetUser().getUsername()).WriteInfo(_user.GetUser().getUsername() + ": checkfile");
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
			case "checkmovie":{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				_threadManage.replace(_threadId, "checkmovie");
				_threadManage.replace((long) 0, "true");
				if (_user.GetUser() == null)
				{
					SendString("failure");
					_threadManage.replace(_threadId, "run");
					_threadManage.replace((long) 0, "true");
					break;
				}
				String folder;
				folder = SFileLocation.MovieFolder.GetPath() + _user.GetUser().getUsername();
				
				
				folder += _split + message[1];
				List <String> content =  new GetFile().getFile(folder);
				List <byte[]> cont = new ArrayList <byte[]>();
				int size = content.size();
				String mess = String.valueOf(size); 
				for (int i = 0; i < size; i++)
				{
					try {
						cont.add(content.get(i).getBytes("UTF-8"));
					} catch (UnsupportedEncodingException e) {
						// TODO Auto-generated catch block
						new SUserLog(_user.GetUser().getUsername()).WriteError(e.toString()); 
						SendString("failure");
						_threadManage.replace(_threadId,  "run");
						_threadManage.replace((long) 0, "true");
						break;
					}
					mess += ' ' + String.valueOf(cont.get(i).length);
				}
				SendString(mess);
				try {
					RecvString();
				} catch (IOException e1) {
					
				}
				for (int i = 0; i < size; i++)
				{
					try {
						SendByte(content.get(i).getBytes("UTF-8"));
					} catch (UnsupportedEncodingException e) {
						// TODO Auto-generated catch block
						new SUserLog(_user.GetUser().getUsername()).WriteError(e.toString()); 
						SendString("failure");
						_threadManage.replace(_threadId,  "run");
						_threadManage.replace((long) 0, "true");
						break;
					}
					try {
						RecvString();
					} catch (IOException e) {
						
					}
				}
				new SUserLog(_user.GetUser().getUsername()).WriteInfo(_user.GetUser().getUsername() + ": checkmovie");
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			case "handhandle":
			{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				_threadManage.replace(_threadId, "handhandle");
				_threadManage.replace((long) 0, "true");
				if (_user.GetUser() == null)
				{
					SendString("failure");
					_threadManage.replace(_threadId, "run");
					_threadManage.replace((long) 0, "true");
					break;
				} 
				SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy_MM_dd_HH_mm_ss_");
				String time = simpleDateFormat.format(new Date());
				DataHandler run = new DataHandler();
				String userLocation = SFileLocation.UserFolder.GetUserPath(_user.GetUser().getUsername());
				String movieLocation = SFileLocation.MovieFolder.GetUserPath(_user.GetUser().getUsername());
				run.Handler(movieLocation + "hand" + _split + message[1], userLocation + message[2], 
							userLocation + time + "left_hand_result.txt", 
							userLocation + time + "right_hand_result.txt",
							userLocation + "lefthand_history.txt", 
							userLocation + "righthand_history.txt");
				SendString(time);
				new SUserLog(_user.GetUser().getUsername()).WriteInfo(_user.GetUser().getUsername() + ": handhandle");
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
			case "shoulderhandle":
			{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				_threadManage.replace(_threadId, "shoulderhandle");
				_threadManage.replace((long) 0, "true");
				if (_user.GetUser() == null)
				{
					SendString("failure");
					_threadManage.replace(_threadId, "run");
					_threadManage.replace((long) 0, "true");
					break;
				} 
				SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy_MM_dd_HH_mm_ss_");
				String time = simpleDateFormat.format(new Date());
				ShoulderDataHandler run = new ShoulderDataHandler();
				String userLocation = SFileLocation.UserFolder.GetUserPath(_user.GetUser().getUsername());
				run.Handler(userLocation + message[1], 
							userLocation + time + "shoulder_result.txt", 
							userLocation + "shoulder_history.txt");
				SendString(time);
				new SUserLog(_user.GetUser().getUsername()).WriteInfo(_user.GetUser().getUsername() + ": shoulderhandle");
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
			case "pushuphandle":
			{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				_threadManage.replace(_threadId, "shoulderhandle");
				_threadManage.replace((long) 0, "true");
				if (_user.GetUser() == null)
				{
					SendString("failure");
					_threadManage.replace(_threadId, "run");
					_threadManage.replace((long) 0, "true");
					break;
				} 
				SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy_MM_dd_HH_mm_ss_");
				String time = simpleDateFormat.format(new Date());
				PushupDataHandler run = new PushupDataHandler();
				String userLocation = SFileLocation.UserFolder.GetUserPath(_user.GetUser().getUsername());
				run.Handler(userLocation + message[1], 
							userLocation + time + "pushup_result.txt", 
							userLocation + "pushup_history.txt");
				SendString(time);
				new SUserLog(_user.GetUser().getUsername()).WriteInfo(_user.GetUser().getUsername() + ": pushuphandle");
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
			case "sendmovie":{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				_threadManage.replace(_threadId, "sendmovie");
				_threadManage.replace((long) 0, "true");
				if (_user.GetUser() == null)
				{
					SendString("failure");
					_threadManage.replace(_threadId, "run");
					break;
				}
				String filename = message[1];
				int size = Integer.valueOf(message[2]).intValue();
				int lastSize = Integer.valueOf(message[3]).intValue();
				String flag = message[4]; 
				String folder = SFileLocation.MovieFolder.GetPath() + _user.GetUser().getUsername() + _split + flag; 
				 
				System.out.println(folder);
				String path = folder + _split + filename;
				SendString("success");
				SFile sfile = new SFile(path);
				for (int i = 0; i < size; i++)
				{
					int length = 1000;
					if (i == size - 1)
					{
						length = lastSize;
					}
					try {
						sfile.WriteFileBinary(RecvByte(length));
					} catch (IOException e) {
						new SUserLog(_user.GetUser().getUsername()).WriteError(e.toString());
						SendString("failure");
						_threadManage.replace(_threadId,  "run");
						_threadManage.replace((long) 0, "true");
						break;
					}
					SendString("success");
				}
				sfile = null;
				System.gc();
				new SUserLog(_user.GetUser().getUsername()).WriteInfo( _user.GetUser().getUsername() + ": send the movie " + filename);
				SendString("success");
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
			case "recvmovie":
			{
				if (_threadManage.get(_threadId) == "exit")
				{
					break;
				}
				_threadManage.replace(_threadId, "recvmovie");
				_threadManage.replace((long) 0, "true");
				if (_user.GetUser() == null)
				{
					SendString("failure");
					_threadManage.replace(_threadId, "run");
					_threadManage.replace((long) 0, "true");
					break;
				}
				String filename = message[1];
				String flag = message[2];
				String folder = SFileLocation.MovieFolder.GetPath() + _user.GetUser().getUsername() + _split + flag;
				
				
				String path = folder + _split + filename;
				SFile sfile = new SFile(path);
				List<byte[]> content = new ArrayList<byte[]>();
				byte[] buf = null;
				int lastSize = 0;
				try {
					while((buf = sfile.ReadFileBinary()).length != 0 )
					{
						content.add(buf);
						lastSize = buf.length;
					}
				} catch (IOException e) {
					// TODO Auto-generated catch block
					new SUserLog(_user.GetUser().getUsername()).WriteError(e.toString()); 
					SendString("failure");
					_threadManage.replace(_threadId,  "run");
					_threadManage.replace((long) 0, "true");
					break;
				}
				int size = content.size();
				String mess = String.valueOf(size) + ' ' + String.valueOf(lastSize);
				
				SendString(mess);
				for (int i = 0; i < size; i++)
				{
					String recvMessage = "";
					try {
						recvMessage = RecvString();
					} catch (IOException e) {
						
					}
					if (!recvMessage.equals("success"))
					{
						_threadManage.replace(_threadId, "run");
						_threadManage.replace((long) 0, "true");
						break;
					}
					SendByte(content.get(i));
				}
				new SUserLog(_user.GetUser().getUsername()).WriteInfo( _user.GetUser().getUsername() + ": receive the movie " + filename);
				_threadManage.replace(_threadId, "run");
				_threadManage.replace((long) 0, "true");
				break;
			}
			
		
		}
	}

	private void Mkdir(String username) throws IOException {
		String folder = SFileLocation.UserFolder.GetPath() + _user.GetUser().getUsername();
		File filefolder = new File(folder);
		if (!filefolder.isDirectory())
		{
			filefolder.mkdir();
		}
		String folder0 = SFileLocation.MovieFolder.GetPath() + _user.GetUser().getUsername();
		File filefolder0 = new File(folder0);
		if (!filefolder0.isDirectory())
		{
			filefolder0.mkdir();
		}
		String folder1 = folder0 + _split + "body";
		String folder2 = folder0 + _split + "arm";
		String folder3 = folder0 + _split + "hand";
		File filefolder1 = new File(folder1);
		if (!filefolder1.isDirectory())
		{
			filefolder1.mkdir();
		}
		File filefolder2 = new File(folder2);
		if (!filefolder2.isDirectory())
		{
			filefolder2.mkdir();
		}
		File filefolder3 = new File(folder3);
		if (!filefolder3.isDirectory())
		{
			filefolder3.mkdir();
		}
		File oldfile1 = new File(SFileLocation.SystemFolder.GetPath() + "DefaultFile" + _split + "pushup_history.txt");		
		File newfile1 = new File(folder + _split + "pushup_history.txt");
		newfile1.createNewFile();
		MoveFile(oldfile1, newfile1);
		
		File oldfile2 = new File(SFileLocation.SystemFolder.GetPath() + "DefaultFile" + _split + "lefthand_history.txt");		
		File newfile2 = new File(folder + _split + "lefthand_history.txt");
		newfile2.createNewFile();
		MoveFile(oldfile2, newfile2);
		
		File oldfile3 = new File(SFileLocation.SystemFolder.GetPath() + "DefaultFile" + _split + "righthand_history.txt");		
		File newfile3 = new File(folder + _split + "righthand_history.txt");
		newfile3.createNewFile();
		MoveFile(oldfile3, newfile3);
		
		File oldfile4 = new File(SFileLocation.SystemFolder.GetPath() + "DefaultFile" + _split + "shoulder_history.txt");		
		File newfile4 = new File(folder + _split + "shoulder_history.txt");
		newfile4.createNewFile();
		MoveFile(oldfile4, newfile4);
		
		File oldfile5 = new File(SFileLocation.SystemFolder.GetPath() + "DefaultFile" + _split + "armplan.txt");		
		File newfile5 = new File(folder + _split + "armplan.txt");
		newfile5.createNewFile();
		MoveFile(oldfile5, newfile5);
		
		File oldfile6 = new File(SFileLocation.SystemFolder.GetPath() + "DefaultFile" + _split + "handplan.txt");		
		File newfile6 = new File(folder + _split + "handplan.txt");
		newfile6.createNewFile();
		MoveFile(oldfile6, newfile6);
		
		File oldfile7 = new File(SFileLocation.SystemFolder.GetPath() + "DefaultFile" + _split + "bodyplan.txt");		
		File newfile7 = new File(folder + _split + "bodyplan.txt");
		newfile7.createNewFile();
		MoveFile(oldfile7, newfile7);
	}
	
	@SuppressWarnings("resource")
	private void MoveFile(File src, File dest) throws IOException
	{
        int length=2097152;
		FileInputStream in=new FileInputStream(src);
        FileOutputStream out=new FileOutputStream(dest);
        FileChannel inC=in.getChannel();
        FileChannel outC=out.getChannel();
        while(true){
            if(inC.position()==inC.size()){
                inC.close();
                outC.close();
                return;
            }
            if((inC.size()-inC.position())<20971520)
                length=(int)(inC.size()-inC.position());
            else
                length=20971520;
            inC.transferTo(inC.position(),length,outC);
            inC.position(inC.position()+length);
        }
	}
}
