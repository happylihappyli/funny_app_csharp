
var password="test";

function upload_click(data){
    var file=s_ui.Text_Read("txt_upload");
    var path=s_ui.Combox_Text("txt_ftp_path")+"/"+s_file.File_Short_Name(file);

    var hosts=s_ui.Text_Read("txt_host");
    s_net.ftp_upload(hosts,"test",password,"22",file,path,"set_status","show_error");
}


function download_click(data){
    var file=s_ui.Combox_Text("txt_ftp_path");
    var local=s_ui.Text_Read("txt_local")+"\\"+s_file.File_Short_Name(file);

    var hosts=s_ui.Text_Read("txt_host");
    s_net.ftp_download(hosts,"test",password,"22",file,local,"set_status","show_error");
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
    var strSplit=data.split("|");
    //s_ui.Text_Set("txt_error",data);
    for (var i=0;i<strSplit.length;i++){
        s_ui.Combox_Add("txt_ftp_path",strSplit[i]);
    }
}


function file_open(){
    var strLine=s_file.File_Open();
    s_ui.Text_Set("txt_upload",strLine);
}

function next_click(data){
    
    s_ui.Run_JS(data+".js");
    s_ui.close();
}

s_ui.Label_Init("lb_site","服务器：",10,50);
s_ui.Text_Init("txt_host","robot6.funnyai.com",100,50,500,30);

s_ui.Label_Init("lb_upload","上传文件：",10,100);


s_ui.Text_Init("txt_upload","",100,100,500,30);
s_ui.Button_Init("b2_1","选择文件",620,100,150,30,"file_open","");

s_ui.Label_Init("lb_ftp_path","路径：",10,150);

s_ui.Combox_Init("txt_ftp_path","/upload/",100,150,500,30);
s_ui.Combox_Add("txt_ftp_path","/upload/");

s_ui.Button_Init("b_upload","上传",620,150,150,30,"upload_click","");


s_ui.TextBox_Init("txt_error","先选择文件，然后点击上传\r\n\r\n 上传成功，点击下一步",100,200,500,200);

s_ui.Progress_Init("progress1",100,400,500,30);

s_ui.Button_Init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat\\step1");
s_ui.Button_Init("b_next","2.下一步",350,500,200,30,"next_click","Run_Bat\\step2");

s_ui.Show_Form(860,650);

s_ui.Form_Title("第一步：文件上传");

//s_file.AES_Encrypt_File("E:\\sample1.txt","E:\\sample1_en.txt","test");
//s_file.AES_Decrypt_File("E:\\sample1_en.txt","E:\\sample1_de.txt","test");

s_net.ftp_list("robot6.funnyai.com","test","test",22,"/upload","callback_ftp_list");

