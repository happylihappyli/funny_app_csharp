
//按钮点击
function upload_click(data){
    var password=s_ui.text_read("txt1");
    var file=s_ui.text_read("txt_upload");
    var path=s_ui.combox_text("txt_ftp_path")+"/"+s_file.File_Short_Name(file);

    var hosts=s_ui.combox_text("cb_host");
    s_net.ftp_upload(hosts,"root",password,"22",file,path,"set_status","show_error");
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
    s_file.save("D:/Net/Web/password_upload_1.txt",strLine);
    
}

//读取密码
function read_password(data){
    var strLine=s_file.read("D:/Net/Web/password_upload_1.txt");
    var strPassword=s_string.decrypt_private_key("D:/Net/Web/id_rsa",strLine);
    //s_ui.msg(strPassword);
    s_ui.text_set("txt1",strPassword);
}

function file_open(){
    var strLine=s_file.File_Open();
    
    s_ui.listbox_add("txt_upload",strLine);
    s_ui.listbox_item_selected("txt_upload",s_ui.ListBox_Item_Size()-1);
    
}

function read(strFile){
    s_ui.listbox_clear("txt_upload");
    s_ui.listbox_from_file("txt_upload",s_sys.App_Path()+"\\config\\"+strFile);
}


s_ui.label_init("lb_ftp_host","上传路径：",10,200);
s_ui.combox_init("cb_host","robot6.funnyai.com",100,10,500,30);
s_ui.combox_add("cb_host","robot6.funnyai.com");
s_ui.combox_add("cb_host","robot5.funnyai.com");

s_ui.label_init("lb_password","FTP密码：",10,70);

s_ui.password_init("txt1","",100,65,100,30);


s_ui.button_init("b2_2","读取密码",250,60,100,30,"read_password","");
s_ui.button_init("b2_1","保存密码",350,60,100,30,"save_password","");


s_ui.label_init("lb_upload","上传文件：",10,100);

s_ui.text_init("txt_upload",s_sys.args(1),100,100,500,30);
//s_ui.listbox_add("txt_upload",sys.args(1));


s_ui.button_init("b_select","选择文件",100,150,200,30,"file_open","");


s_ui.label_init("lb_ftp_path","上传路径：",10,200);

s_ui.combox_init("txt_ftp_path","/root/happyli",100,200,500,30);
s_ui.combox_add("txt_ftp_path","/root/happyli");
s_ui.combox_add("txt_ftp_path","/root/happyli/lib");


s_ui.button_init("b_upload","upload",100,250,200,30,"upload_click","");


s_ui.textbox_init("txt_error","错误信息：",100,300,500,200);


s_ui.progress_init("progress1",100,500,500,30);

s_ui.Show_Form(700,600);

s_ui.Form_Title("upload："+s_sys.args(1));

read_password("");





