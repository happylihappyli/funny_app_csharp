
//按钮点击
function upload_click(data){
    var password=s_ui.Text_Read("txt1");
    var file=s_ui.Text_Read("txt_upload");
    var path=s_ui.Combox_Text("txt_ftp_path")+"/"+s_file.File_Short_Name(file);

    var hosts=s_ui.Text_Read("txt_host");
    s_net.Net_Upload(hosts,"test",password,"22",file,path,"set_status","show_error");
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

function file_open(){
    var strLine=s_file.File_Open();
    
    s_ui.ListBox_Add("list_upload",strLine);
    s_ui.ListBox_Item_Selected("list_upload",s_ui.ListBox_Item_Size()-1);
}


function file_open_config(data){

    s_ui.Run_Cmd("Notepad++.exe "+sys.App_Path()+"\\config\\"+data);
}


function callback_ftp_list(data){
    s_ui.Text_Set("txt_error",data);
}


function file_open(){
    var strLine=s_file.File_Open();
    s_ui.Text_Set("txt_upload",strLine);
}

s_ui.Text_Init("txt_host","robot6.funnyai.com",100,10,500,30);


s_ui.Label_Init("lb_password","FTP密码：",10,70);

s_ui.Password_Init("txt1","test",100,65,100,30);


s_ui.Label_Init("lb_upload","上传文件：",10,100);


s_ui.Text_Init("txt_upload","",100,100,500,30);
s_ui.Button_Init("b2_1","选择文件",100,150,200,30,"file_open","");

s_ui.Label_Init("lb_ftp_path","路径：",10,200);

s_ui.Combox_Init("txt_ftp_path","/upload/",100,200,500,30);
s_ui.Combox_Add("txt_ftp_path","/root/happyli/lib/");


s_ui.Button_Init("b3_1","upload",100,250,200,30,"upload_click","");


s_ui.TextBox_Init("txt_error","错误信息：",100,300,500,200);


s_ui.Progress_Init("progress1",100,500,500,30);

s_ui.Show_Form(800,600);

s_ui.Form_Title("上传文件Test");

//s_file.AES_Encrypt_File("E:\\sample1.txt","E:\\sample1_en.txt","test");
//s_file.AES_Decrypt_File("E:\\sample1_en.txt","E:\\sample1_de.txt","test");

s_net.ftp_list("robot6.funnyai.com","test","test",22,"/upload","callback_ftp_list");

