package perfit.consistency;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

public class GetFile {
	
    public List<String> getFile(String path){   
        File file = new File(path);   
        File[] array = file.listFiles();   
        List<String> files = new ArrayList<String>(); 
        for(int i=0;i<array.length;i++){   
            if(array[i].isFile()){ 
            	files.add(array[i].getName());
            }
        }
        return files;
    }   
    
}
