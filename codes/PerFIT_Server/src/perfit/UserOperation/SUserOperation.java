package perfit.UserOperation;

import java.util.ArrayList;
import java.util.List;

import perfit.DAO.UserDao;
import perfit.DAO.UserDaoImp;
import perfit.util.model.User;

public class SUserOperation {
	private User _user;
	private static List <String> _userList = new ArrayList<String>();
	private UserDao _dao;
	public SUserOperation()
	{
		_user = null;
		_dao = new UserDaoImp();
	}
	
	public User GetUser()
	{
		return _user;
	}
	
	public void RemoveUser(User user) {		
		_userList.remove(_user.getUsername());
	}
	
	public boolean Login(String username, String password)
	{
		if (_userList.indexOf(username) != -1)
		{
			return false;
		}
		_user = _dao.find(username, password);
		if (_user == null)
		{
			return false;
		}
		_userList.add(username);
		return true;
	}

	public boolean Register(String username, String password)
	{
		if (!_dao.create(username, password))
		{
			return false;
		}
		_user = _dao.find(username, password);
		_userList.add(username);
		return true;
	}
	
	public boolean ModifyInfo(String username, double height, double weight)
	{
		User user = _user;
		if (!_user.getUsername().equals(username))
		{
			return false;
		}
		
		user.setHeight(height);
		user.setWeight(weight);
		if (_dao.update(user))
		{
			return false;
		}
		_user = user;
		return true;
	}
	
	public boolean ModifyPass(String username, String password)
	{
		User user = _user;
		if (!_user.getUsername().equals(username))
		{
			return false;
		}
		System.out.println(username);
		System.out.println(password);
		user.setPassword(password);
		if (_dao.update(user))
		{
			return false;
		}
		_user = user;
		return true;
	}
}
