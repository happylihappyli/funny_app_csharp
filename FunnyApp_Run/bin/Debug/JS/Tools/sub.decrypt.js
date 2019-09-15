
var file1=s_sys.Value_Read("file1");// s_ui.Text_Read("txt_file2");
var file2=s_sys.Value_Read("file2");//s_ui.Text_Read("txt_file3");

var key="test";
var step=1024;
s_file.delete(file2);

var index=0;
s_file.Read_Begin(file1,"file1");
var strLine=s_file.Read_Line("file1");
while(strLine!=null){
    base64=s_string.AES_Decrypt(strLine,key);
    var length=s_file.Bin_Write(file2,index,base64);
    index+=length;
    strLine=s_file.Read_Line("file1");
}
s_file.Read_End("file1");