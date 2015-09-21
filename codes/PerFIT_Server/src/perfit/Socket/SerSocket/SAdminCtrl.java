package perfit.Socket.SerSocket;


import java.util.Map;
import java.util.Map.Entry;
import java.util.Scanner;


public class SAdminCtrl extends Thread{
	private Map<Long, String> _threadManage;
	public SAdminCtrl(Map<Long, String> threadManage)
	{
		_threadManage = threadManage;		
	}
	
	public void run()
	{
		String content;
		while (true)
		{
			
			@SuppressWarnings("resource")
			Scanner scanner = new Scanner(System.in);
			content = scanner.nextLine();
			String [] cont = content.split("\\s+");
			
			switch (cont[0])
			{
				case "scan":
					for (Entry<Long, String> entry : _threadManage.entrySet())
					{						  
					    System.out.println("Thread ID = " + entry.getKey() + ", State = " + entry.getValue());					  
					}  
					break;
				case "kill":
					Long Id = (long) Integer.parseInt(cont[1]);
					if (_threadManage.containsKey(Id))
					{
						_threadManage.replace(Id, "exit");
					}
					break;
				default:
					break;
			}
		}
	}
}
