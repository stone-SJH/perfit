package perfit.util.model;

import java.util.Date;

public class User {
	private int ID;
	private String username;
	private String password;
	private String nickname;
	private String sex;
	private int age;
	private double height;
	private double weight;
	private double armspread;
	private Date date;
	private String clientpath;
	
	
	public String getClientpath() {
		return clientpath;
	}
	public void setClientpath(String clientpath) {
		this.clientpath = clientpath;
	}
	public int getID() {
		return ID;
	}
	public void setID(int iD) {
		ID = iD;
	}
	public String getUsername() {
		return username;
	}
	public void setUsername(String username) {
		this.username = username;
	}
	public String getPassword() {
		return password;
	}
	public void setPassword(String password) {
		this.password = password;
	}
	public String getNickname() {
		return nickname;
	}
	public void setNickname(String nickname) {
		this.nickname = nickname;
	}
	public String getSex() {
		return sex;
	}
	public void setSex(String sex) {
		this.sex = sex;
	}
	public int getAge() {
		return age;
	}
	public void setAge(int age) {
		this.age = age;
	}
	public double getHeight() {
		return height;
	}
	public void setHeight(double height) {
		this.height = height;
	}
	public double getWeight() {
		return weight;
	}
	public void setWeight(double weight) {
		this.weight = weight;
	}
	public double getArmspread() {
		return armspread;
	}
	public void setArmspread(double armspread) {
		this.armspread = armspread;
	}
	public Date getDate() {
		return date;
	}
	public void setDate(Date date) {
		this.date = date;
	}	

}
