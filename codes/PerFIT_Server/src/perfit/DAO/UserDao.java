package perfit.DAO;

import perfit.util.model.User;

public interface UserDao {
	
	public User find(int id);
	
	public User find(String username, String password);
	
	public boolean create(String username, String password);
	
	public boolean create(User user);
	
	public boolean delete(int id);
	
	public boolean update(User user);
}
