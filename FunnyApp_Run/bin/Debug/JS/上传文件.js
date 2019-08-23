
//按钮点击
function upload_click(data){
    var password=sys.Get_Text("txt1");
    var file=sys.ListBox_Item_Text("list_upload");
    var path=sys.Combox_Text("txt_ftp_path")+"/"+sys.File_Short_Name(file);

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

function read(strFile){
    sys.ListBox_Clear("list_upload");
    sys.ListBox_From_File("list_upload",sys.App_Path()+"\\config\\"+strFile);
}


function file_open_config(data){

    sys.Run_Cmd("Notepad++.exe "+sys.App_Path()+"\\config\\upload_js.txt");
}


sys.Add_Text("txt_host","robot6.funnyai.com",100,10,500,30);

sys.Label_Init("lb_password","FTP密码：",10,70);

sys.Add_Password("txt1","",100,65,100,30,"");


sys.Button_Init("b2_2","读取密码",250,60,100,30,"read_password","");
sys.Button_Init("b2_1","保存密码",350,60,100,30,"save_password","");


sys.Label_Init("lb_upload","上传文件：",10,100);

sys.Add_ListBox("list_upload","E:\\happyli\\Jar\\line_java\\Line_Java.jar",100,100,500,100);
sys.ListBox_From_File("list_upload",sys.App_Path()+"\\config\\upload.txt");

sys.Button_Init("b_read1","jar",610,100,100,30,"read","upload.txt");
sys.Button_Init("b_read2","js",610,150,100,30,"read","upload_js.txt");

sys.Button_Init("b_edit_1","e upload",610,200,100,30,"file_open_config","upload.txt");
sys.Button_Init("b_edit_2","e upload_js",610,250,100,30,"file_open_config","upload_js.txt");

sys.Label_Init("lb_ftp_path","路径：",10,200);

sys.Add_Combox("txt_ftp_path","/root/happyli",100,200,500,30);
sys.Combox_Add("txt_ftp_path","/root/happyli/lib/");


sys.Button_Init("b3_1","upload",100,250,200,30,"upload_click","");


sys.TextBox_Init("txt_error","错误信息：",100,300,500,200);


sys.Add_Progress("progress1",100,500,500,30);

sys.Show_Form(800,600);

sys.Form_Title("上传文件");

read_password("");





