
//按钮点击
function upload_click(data){
    var user=s_ui.text_read("txt_user");
    var password=s_ui.text_read("txt1");
    var file=s_ui.text_read("txt_upload");//s_ui.listbox_text("list_upload");
    var path=s_ui.combox_text("txt_ftp_path")+"/"+s_file.File_Short_Name(file);

    var hosts=s_ui.text_read("txt_host");
    s_net.ftp_upload(hosts,user,password,"22",file,path,"set_status","show_error");
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
    var strLine=s_string.encrypt_public_key("D:/Net/Web/public/id_rsa_happyli.pem.pub",strPassword);
    s_file.save("D:/Net/Web/password_upload_1.txt",strLine);
    
}

//读取密码
function read_password(data){
    var strLine=s_file.read("D:/Net/Web/password_upload_1.txt");
    var strPassword=s_string.decrypt_private_key("D:/Net/Web/id_rsa",strLine);
    s_ui.text_set("txt1",strPassword);
}

function file_open(){
    var strLine=s_file.File_Open();
    
    s_ui.listbox_add("list_upload",strLine);
    s_ui.listbox_item_selected("list_upload",s_ui.ListBox_Item_Size()-1);
    
}

function read(strFile){
    s_ui.listbox_clear("list_upload");
    s_ui.listbox_from_file("list_upload",s_sys.path_app()+"\\config\\"+strFile);
}


function file_open_config(data){

    s_ui.Run_Cmd("Notepad++.exe "+s_sys.path_app()+"\\config\\"+data);
}

function wall9(data){
    s_ui.text_set("txt_host","80.240.30.201");
}

function jrx(data){
    s_ui.text_set("txt_host","172.16.101.13");
}

function listbox_change(data){
    
    var file=s_ui.listbox_text("list_upload");
    s_ui.text_set("txt_upload",file);
}

s_ui.text_init("txt_host","robot6.funnyai.com",100,10,500,30);

s_ui.button_init("wall9","wall9",610,10,100,30,"wall9","");

s_ui.button_init("jrx","jrx",710,10,100,30,"jrx","");



s_ui.label_init("lb_name","用户名：",10,39);

s_ui.text_init("txt_user","root",100,35,100,30);


s_ui.label_init("lb_password","FTP密码：",10,70);

s_ui.password_init("txt1","",100,65,100,30);


s_ui.button_init("b2_2","读取密码",250,60,100,30,"read_password","");
s_ui.button_init("b2_1","保存密码",350,60,100,30,"save_password","");


s_ui.label_init("lb_upload","上传文件：",10,100);

s_ui.text_init("txt_upload","",100,100,500,30);

s_ui.listbox_init("list_upload",100,150,500,100);

s_ui.listbox_init_event("list_upload","listbox_change");

s_ui.listbox_from_file("list_upload",s_sys.path_app()+"\\config\\upload.txt");


s_ui.button_init("b_read1","jar",610,100,100,30,"read","upload.txt");
s_ui.button_init("b_read2","js",610,150,100,30,"read","upload_js.txt");

s_ui.button_init("b_edit_1","e upload",610,200,100,30,"file_open_config","upload.txt");
s_ui.button_init("b_edit_2","e upload_js",610,250,100,30,"file_open_config","upload_js.txt");

s_ui.label_init("lb_ftp_path","路径：",10,250);

s_ui.combox_init("txt_ftp_path","/root/happyli",100,250,500,30);
s_ui.combox_add("txt_ftp_path","/root/happyli");
s_ui.combox_add("txt_ftp_path","/root/happyli/lib");


s_ui.button_init("b3_1","upload",610,300,100,30,"upload_click","");


s_ui.textbox_init("txt_error","错误信息：",100,300,500,200);


s_ui.progress_init("progress1",100,550,500,30);

s_ui.show_form(860,700);

s_ui.Form_Title("上传文件");

read_password("");


