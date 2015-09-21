package perfit.DAO;

import java.util.List;

import perfit.DAO.UserDao;
import perfit.util.model.User;
import perfit.util.HibernateSessionFactory;

import org.hibernate.Query;
import org.hibernate.Session;
import org.hibernate.Transaction;


public class UserDaoImp implements UserDao {
	
	private Transaction tx;
	private Session session;

	@Override
	public User find(int id) {
		// TODO Auto-generated method stub
		session=HibernateSessionFactory.getSession();
		tx=session.beginTransaction();
		User item=(User)session.get(User.class, id);
		tx.commit();
		session.close();
		return item;
	}

	@Override
	public User find(String username, String password) {
		// TODO Auto-generated method stub
		session=HibernateSessionFactory.getSession();
		tx=session.beginTransaction();
		String sql="from User as u where u.username='"+username+"' and u.password='"+password+"'";
		Query query=session.createQuery(sql);
		@SuppressWarnings("unchecked")
		List<User> list=query.list();
		tx.commit();
		session.close();
		if (list!=null & list.size()>0){
			return (User)list.get(0);
		}
		else return null;
	}

	@Override
	public boolean create(String username, String password) {
		// TODO Auto-generated method stub
		session=HibernateSessionFactory.getSession();
		tx=session.beginTransaction();
		String sql="from User as u where u.username='"+username+"'";
		Query query=session.createQuery(sql);
		@SuppressWarnings("unchecked")
		List<User> list=query.list();
		if (list.size()==0){
			User new_user = new User();
			new_user.setUsername(username);
			new_user.setPassword(password);
			session.save(new_user);
			tx.commit();
			session.close();
			return true;
		}
		tx.commit();
		session.close();
		return false;
	}
	
	public boolean create(User user){
		session=HibernateSessionFactory.getSession();
		tx=session.beginTransaction();
		String username = user.getUsername();
		String sql="from User as u where u.username='"+username+"'";
		Query query=session.createQuery(sql);
		@SuppressWarnings("unchecked")
		List<User> list=query.list();
		if (list.size()==0){
			session.save(user);
			tx.commit();
			session.close();
			return true;
		}
		tx.commit();
		session.close();
		return false;
	}

	@Override
	public boolean delete(int id) {
		// TODO Auto-generated method stub
		session=HibernateSessionFactory.getSession();
		tx=session.beginTransaction();
		User item=(User)session.get(User.class, id);
		session.delete(item);
		tx.commit();
		session.close();
		return true;
	}

	@Override
	public boolean update(User user) {
		// TODO Auto-generated method stub
		session=HibernateSessionFactory.getSession();
		tx=session.beginTransaction();
		session.update(user);
		tx.commit();
		session.close();
		return false;
	}

}
