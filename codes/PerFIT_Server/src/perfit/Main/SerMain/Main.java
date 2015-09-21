package perfit.Main.SerMain;

import java.io.FileNotFoundException;
import java.io.IOException;

import perfit.Socket.SerSocket.SSocket;

public class Main {

	public static void main(String[] args) throws FileNotFoundException, IOException {
		// TODO Auto-generated method stub
		new SSocket().StartService();
	}

}
