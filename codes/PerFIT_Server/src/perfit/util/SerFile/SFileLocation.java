package perfit.util.SerFile;

public enum SFileLocation {
	UserFolder("File/"),
	MovieFolder("Movie/"),
	SystemFolder("System/");
	private String _path; 
	public String GetPath() {
		return _path;
	}
	public String GetUserPath(String username)
	{
		return _path + username + "/";
	}
	
	public String GetSystemLogPath()
	{
		return _path + "log";
	}
	private SFileLocation(String path)
	{
		_path = path;
	}
}
