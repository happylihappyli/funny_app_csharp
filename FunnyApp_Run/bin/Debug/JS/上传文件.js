
//按钮点击
function upload_click(data){
    var password=sys.Get_Text("txt1");
    var file=sys.ListBox_Item_Text("list_upload");
    var path=sys.Get_Text("txt_ftp_path")+"/"+sys.File_Short_Name(file);

    var hosts=sys.Get_Text("txt_host");
    sys.Net_Upload(hosts,"root",password,"22",file,path,"set_status","show_error");
}

function set_status(data){
    var strSplit=data.split(",");
    sys.Show_ProgressBar("progress1",strSplit[1],strSplit[0]);
}

var log_error="";
function show_error(data){
    log_error+=data+"\n";
    sys.Show_Text("txt_error",log_error);
}


//保存密码
function save_password(data){
    var strPassword=sys.Get_Text("txt1");
    var strLine=sys.encrypt_public_key("D:/Net/Web/id_rsa.pem.pub",strPassword);
    sys.File_Save("D:/Net/Web/password_upload_1.txt",strLine);
    
}

//读取密码
function read_password(data){
    var strLine=sys.File_Read("D:/Net/Web/password_upload_1.txt");
    var strPassword=sys.decrypt_private_key("D:/Net/Web/id_rsa",strLine);
    sys.Show_Text("txt1",strPassword);
}

function file_open(){
    var strLine=sys.File_Open();
    
    sys.ListBox_Add("list_upload",strLine);
    sys.ListBox_Item_Selected("list_upload",sys.ListBox_Item_Size()-1);
    
}



sys.Add_Text("txt_host","robot6.funnyai.com",100,10,500,30);


sys.Add_Label("lb_password","FTP密码：",10,70);

sys.Add_Password("txt1","",100,65,100,30,"");


sys.Add_Button("b2_2","读取密码",250,60,100,30,"read_password","");
sys.Add_Button("b2_1","保存密码",350,60,100,30,"save_password","");



sys.Add_Label("lb_upload","上传文件：",10,100);

sys.Add_ListBox("list_upload","E:\\happyli\\Jar\\line_java\\Line_Java.jar",100,100,500,100);
sys.ListBox_Add("list_upload","E:\\happyli\\Jar\\line_java\\Line_Java.jar");
sys.ListBox_Add("list_upload","E:\\happyli\\Jar\\sync_struct\\mv_sync_struct.jar");
sys.ListBox_Add("list_upload","E:\\happyli\\Jar\\sync_seg\\mv_sync_seg.jar");
sys.ListBox_Add("list_upload","E:\\happyli\\Jar\\sync_subset\\mv_sync_subset.jar");

sys.Add_Button("b2_1","选择文件",100,150,200,30,"file_open","");


sys.Add_Label("lb_ftp_path","路径：",10,200);

sys.Add_Text("txt_ftp_path","/root/happyli",100,200,500,30);


sys.Add_Button("b3_1","upload",100,250,200,30,"upload_click","");


sys.Add_Text_Multi("txt_error","错误信息：",100,300,500,200);


sys.Add_Progress("progress1",100,500,500,30);

sys.Show_Form(700,600);

sys.Form_Title("上传文件");

read_password("");





