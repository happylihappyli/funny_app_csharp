function sys_field_count(){
    var file1=s_sys.value_read("file1");
    if (file1=="") file1="E:\\sample1.txt";
    
    var line=s_file.read(file1,1);
    var strSplit=line.split("|");
    fields_count=strSplit.length;
    
    var line=s_file.read(file1,1);
    var strSplit=line.split("|");
    var fields_count=strSplit.length;
    var count_field=0;
    
    for (var i=1;i<=fields_count;i++){
        var value=s_file.Ini_Read(file_memo,"selected","check"+i);
        if (value=="1"){
            count_field+=1;
        }
    }
    return count_field;
}