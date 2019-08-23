
//按钮点击
function upload_click(data){
    var password=sys.Get_Text("txt1");
    var file=sys.Get_Text("txt_upload_file");
    var path=sys.Get_Text("txt_ftp_path")+"/"+sys.File_Short_Name(file);
    //"/root/happyli/Line_Java.jar"
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

function power_click(){
    sys.Run_App("rundll32.exe", " shell32.dll,Control_RunDLL powercfg.cpl");
}

function uninstall_click(){
    sys.Run_App("rundll32.exe"," shell32.dll,Control_RunDLL appwiz.cpl,,1");
}


function start_click(){
    //sys.Open_Fold("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\StartUp");
    sys.Run_Cmd("explorer \"C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\StartUp\"");
}

sys.Button_Init("b1_1","电源",100,50,200,30,"power_click","");
sys.Button_Init("b1_2","卸载软件",300,50,200,30,"uninstall_click","");


sys.Button_Init("b2_1","启动目录",300,150,200,30,"start_click","");



sys.TextBox_Init("txt_error","错误信息：",100,300,500,200);


sys.Show_Form(800,600);

sys.Form_Title("控制面板");







