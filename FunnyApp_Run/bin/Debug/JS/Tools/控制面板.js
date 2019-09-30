
//按钮点击
function upload_click(data){
    var password=s_ui.text_read("txt1");
    var file=s_ui.text_read("txt_upload_file");
    var path=s_ui.text_read("txt_ftp_path")+"/"+s_file.File_Short_Name(file);
    //"/root/happyli/Line_Java.jar"
    var hosts=s_ui.text_read("txt_host");
    s_net.Net_Upload(hosts,"root",password,"22",file,path,"set_status","show_error");
}

function set_status(data){
    var strSplit=data.split(",");
    s_ui.progress_show("progress1",strSplit[1],strSplit[0]);
}

var log_error="";
function show_error(data){
    log_error+=data+"\n";
    s_ui.text_set("txt_error",log_error);
}


//保存密码
function save_password(data){
    var strPassword=s_ui.text_read("txt1");
    var strLine=s_string.encrypt_public_key("D:/Net/Web/id_rsa.pem.pub",strPassword);
    s_file.File_Save("D:/Net/Web/password_upload_1.txt",strLine);
    
}

//读取密码
function read_password(data){
    var strLine=s_file.File_Read("D:/Net/Web/password_upload_1.txt");
    var strPassword=s_string.decrypt_private_key("D:/Net/Web/id_rsa",strLine);
    s_ui.text_set("txt1",strPassword);
}

function power_click(){
    s_ui.Run_App("rundll32.exe", " shell32.dll,Control_RunDLL powercfg.cpl");
}

function uninstall_click(){
    s_ui.Run_App("rundll32.exe"," shell32.dll,Control_RunDLL appwiz.cpl,,1");
}


function start_click(){
    //sys.Open_Fold("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\StartUp");
    s_ui.Run_Cmd("explorer \"C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\StartUp\"");
}

s_ui.button_init("b1_1","电源",100,50,200,30,"power_click","");
s_ui.button_init("b1_2","卸载软件",300,50,200,30,"uninstall_click","");


s_ui.button_init("b2_1","启动目录",300,150,200,30,"start_click","");



s_ui.textbox_init("txt_error","错误信息：",100,300,500,200);


s_ui.Show_Form(800,600);

s_ui.Form_Title("控制面板");







