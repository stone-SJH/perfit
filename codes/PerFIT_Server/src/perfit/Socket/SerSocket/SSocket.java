package perfit.Socket.SerSocket;

import java.io.IOException;
import java.net.*;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import perfit.util.SerFile.*;


public class SSocket {	
	private ServerSocket _serverSocket;
	private final int _port = 5555;
	private ExecutorService _pool;
	private List <String> _user;
	private Map <Long, String> _threadManage;
	public SSocket()
	{
		try {
			_serverSocket = new ServerSocket(_port);
			new SSystemLog().WriteInfo("The server has been started.");
			System.out.println("The server has been started.");
			_pool = Executors.newFixedThreadPool(12);
			_user = new ArrayList<String>(); 
			_threadManage = new HashMap<Long, String>();
		} catch (IOException e) {
			new SSystemLog().WriteError(e.toString());
		}
	}
	
	public void StartService()
	{
		try
		{
			_threadManage.put((long) 0, "false");
			_pool.execute(new SAdminWatch(_threadManage));
			_pool.execute(new SAdminCtrl(_threadManage));
			while (true)
			{
				Socket clientSocket = _serverSocket.accept();
				_pool.execute(new STransaction(clientSocket, _user, _threadManage));
			}
		}
		catch (IOException e)
		{
			new SSystemLog().WriteError(e.toString());
		}
		
	}
	
	
}

