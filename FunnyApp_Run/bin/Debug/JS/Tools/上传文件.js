
//按钮点击
function upload_click(data){
    var password=s_ui.Text_Read("txt1");
    var file=s_ui.ListBox_Text("list_upload");
    var path=s_ui.Combox_Text("txt_ftp_path")+"/"+s_file.File_Short_Name(file);

    var hosts=s_ui.Text_Read("txt_host");
    s_net.Net_Upload(hosts,"root",password,"22",file,path,"set_status","show_error");
}

function set_status(data){
    var strSplit=data.split(",");
    s_ui.ProgressBar_Show("progress1",strSplit[1],strSplit[0]);
}

var log_error="";
function show_error(data){
    log_error+=data+"\n";
    s_ui.Text_Set("txt_error",log_error);
}


//保存密码
function save_password(data){
    var strPassword=s_ui.Text_Read("txt1");
    var strLine=s_string.encrypt_public_key("D:/Net/Web/id_rsa.pem.pub",strPassword);
    s_file.File_Save("D:/Net/Web/password_upload_1.txt",strLine);
    
}

//读取密码
function read_password(data){
    var strLine=s_file.File_Read("D:/Net/Web/password_upload_1.txt");
    var strPassword=s_string.decrypt_private_key("D:/Net/Web/id_rsa",strLine);
    s_ui.Text_Set("txt1",strPassword);
}

function file_open(){
    var strLine=s_file.File_Open();
    
    s_ui.ListBox_Add("list_upload",strLine);
    s_ui.ListBox_Item_Selected("list_upload",s_ui.ListBox_Item_Size()-1);
    
}

function read(strFile){
    s_ui.ListBox_Clear("list_upload");
    s_ui.ListBox_From_File("list_upload",sys.App_Path()+"\\config\\"+strFile);
}


function file_open_config(data){

    s_ui.Run_Cmd("Notepad++.exe "+sys.App_Path()+"\\config\\upload_js.txt");
}


s_ui.Text_Init("txt_host","robot6.funnyai.com",100,10,300,30);

s_ui.Label_Init("lb_password","FTP密码：",10,70);

s_ui.Password_Init("txt1","",100,65,100,30);


s_ui.Button_Init("b2_2","读取密码",250,60,100,30,"read_password","");
s_ui.Button_Init("b2_1","保存密码",350,60,100,30,"save_password","");


s_ui.Label_Init("lb_upload","上传文件：",10,100);


s_ui.ListBox_Init("list_upload",100,100,500,100);

s_ui.ListBox_From_File("list_upload",sys.App_Path()+"\\config\\upload.txt");


s_ui.Button_Init("b_read1","jar",610,100,100,30,"read","upload.txt");
s_ui.Button_Init("b_read2","js",610,150,100,30,"read","upload_js.txt");

s_ui.Button_Init("b_edit_1","e upload",610,200,100,30,"file_open_config","upload.txt");
s_ui.Button_Init("b_edit_2","e upload_js",610,250,100,30,"file_open_config","upload_js.txt");

s_ui.Label_Init("lb_ftp_path","路径：",10,200);

s_ui.Combox_Init("txt_ftp_path","/root/happyli",100,200,500,30);
s_ui.Combox_Add("txt_ftp_path","/root/happyli/lib/");


s_ui.Button_Init("b3_1","upload",100,250,200,30,"upload_click","");


s_ui.TextBox_Init("txt_error","错误信息：",100,300,500,200);


s_ui.Progress_Init("progress1",100,500,500,30);

s_ui.Show_Form(800,600);

s_ui.Form_Title("上传文件");

read_password("");


