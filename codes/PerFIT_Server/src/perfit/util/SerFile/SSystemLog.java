package perfit.util.SerFile;

import java.text.SimpleDateFormat;
import java.util.Date;


public class SSystemLog extends SFile{
	private static String _path = SFileLocation.SystemFolder.GetSystemLogPath();
	public SSystemLog() {
		super(_path);
		// TODO Auto-generated constructor stub
	}
	
	public void WriteInfo(String info){
		SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		String time = simpleDateFormat.format(new Date());
		this.WriteLine(time + " Infomation: " + info);
	}
	
	public void WriteError(String err){
		SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		String time = simpleDateFormat.format(new Date());
		this.WriteLine(time + " Error: " + err);
	}
}
