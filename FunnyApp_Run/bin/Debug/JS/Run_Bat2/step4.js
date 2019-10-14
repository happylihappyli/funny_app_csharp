
var password="test";
var index=0;
var flag="uploading";


//检查
function check_upload(data){
    s_time.setTimeout("check_upload",2,"memo");
    if (flag=="finished"){
        upload_sub("");
    }
}

function upload_click(data){
    index=0;
    upload_sub();
}

function upload_sub(){
    flag="uploading";
    
    var size=s_ui.listbox_item_size("list_upload");
    
    if (index>=size){
        index=0;
        return ;
    }
    s_ui.listbox_selected("list_upload",index);
    
    var file=s_ui.listbox_item("list_upload",index);
    var path="/upload/map/"+s_file.File_Short_Name(file);

    var hosts="robot6.funnyai.com";
    
    //s_ui.msg(file);
    s_net.ftp_upload(hosts,"test",password,"22",file,path,"set_status","show_error");
}

function download_click(data){
    var file=s_ui.combox_text("txt_ftp_path");
    var local=s_ui.text_read("txt_local")+"\\"+s_file.File_Short_Name(file);

    var hosts=s_ui.text_read("txt_host");
    s_net.ftp_download(hosts,"test",password,"22",file,local,"set_status","show_error");
}


function set_status(data){
    var strSplit=data.split(",");
    s_ui.progress_show("progress1",strSplit[1],strSplit[0]);
}

var log_error="";
function show_error(data){
    if (data.indexOf("传输完毕")>-1){
        index+=1;
        flag="finished";
        //s_ui.msg(flag);
    }
    log_error+=data+"\n";
    s_ui.text_set("txt_error",log_error);
}



function callback_ftp_list(data){
    var strSplit=data.split("|");
    //s_ui.text_set("txt_error",data);
    for (var i=0;i<strSplit.length;i++){
        s_ui.combox_add("txt_ftp_path",strSplit[i]);
    }
}

function list_file(data){
    var strLine=s_file.File_List_File("D:\\Net\\Web\\Data");
    var strSplit=strLine.split("|");

    for(var i=0;i<strSplit.length;i++){
        var file="D:\\Net\\Web\\Data\\"+strSplit[i];
        s_ui.listbox_add("list_upload",file);
    }
}


function next_click(data){
    //var file=s_ui.combox_text("list_upload");
    //s_sys.value_save("file1",file);
    s_ui.Run_JS(data+".js");
    s_ui.close();
}

s_ui.label_init("lb_upload","上传文件：",10,100);
s_ui.listbox_init("list_upload",100,100,500,200);



s_ui.button_init("b_upload","上传",620,350,150,30,"upload_click","");


s_ui.textbox_init("txt_error","先选择文件，然后点击上传\r\n\r\n 上传成功，点击下一步",100,350,500,200);

s_ui.progress_init("progress1",100,550,500,30);

s_ui.button_init("b_pre","上一步",100,600,200,30,"next_click","Run_Bat2\\step3");
s_ui.button_init("b_next","2.下一步",350,600,200,30,"next_click","Run_Bat2\\step5");

s_ui.Show_Form(860,750);

s_ui.Form_Title("v2 第4步：文件上传");

list_file("");
check_upload("");

