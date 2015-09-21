package perfit.Socket.SerSocket;


import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Map;
import java.util.Map.Entry;


public class SAdminWatch extends Thread{
	private Map<Long, String> _threadManage;
	public SAdminWatch(Map<Long, String> threadManage)
	{
		_threadManage = threadManage;		
	}
	
	public void run()
	{
		while (true)
		{
			if (_threadManage.get((long) 0).equals("true"))
			{
				SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
				String time = simpleDateFormat.format(new Date());
				System.out.println(time + ":");
				for (Entry<Long, String> entry : _threadManage.entrySet())
				{			
					if (entry.getKey() != 0)
					{
						System.out.println("Thread ID = " + entry.getKey() + ", State = " + entry.getValue());
					}
				} 
				_threadManage.replace((long) 0, "false");
			}
		}
	}
}
