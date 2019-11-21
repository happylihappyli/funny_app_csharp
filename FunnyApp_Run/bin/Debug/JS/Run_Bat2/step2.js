
[[[..\\data\\default.js]]]
[[[..\\data\\common_string.js]]]
[[[..\\data\\tcp.js]]]
[[[..\\data\\run_bat_common.js]]]

var file_memo=disk+"\\Net\\Web\\Data\\memo.ini";
var file_ini=disk+"\\Net\\Web\\main.ini";
var friend=s_file.Ini_Read(file_ini,"main","friend_selected");


var password="123ewq!@#EWQ";


function set_status(data){
    var strSplit=data.split(",");
    s_ui.progress_show("progress1",strSplit[1],strSplit[0]);
}

var log_error="";
function show_error(data){
    log_error+=data+"\n";
    s_ui.text_set("txt_error",log_error);
}

function file_open(){
    var strLine=s_file.File_Open();
    
    s_ui.listbox_add("list_upload",strLine);
    s_ui.listbox_item_selected("list_upload",s_ui.ListBox_Item_Size()-1);
}


function file_open_config(data){

    s_ui.Run_Cmd("Notepad++.exe "+sys.App_Path()+"\\config\\"+data);
}


function callback_ftp_list(data){
    s_ui.listbox_clear("txt_ftp_path");
    var strSplit=data.split("|");
    //s_ui.text_set("txt_error",data);
    for (var i=0;i<strSplit.length;i++){
        s_ui.listbox_add("txt_ftp_path",strSplit[i]);
    }
}


function file_open(){
    var strLine=s_file.File_Open();
    s_ui.text_set("txt_upload",strLine);
}

function next_click(data){
    var file=s_ui.listbox_text("txt_ftp_path");
    if (file!=""){
        s_sys.value_save("file2",file);
        s_file.Ini_Save(file_memo,"main","file2",file);
    }
    s_ui.Run_JS(data+".js");
    s_ui.close();
}



s_ui.label_init("lb_ftp_path","数据文件：",100,50);

s_ui.listbox_init("txt_ftp_path",100,100,500,300);
s_ui.listbox_add("txt_ftp_path","正在读取...");


s_ui.button_init("b_pre","上一步",100,500,200,30,"next_click","Run_Bat2\\step1");
s_ui.button_init("b_next","下一步",350,500,200,30,"next_click","Run_Bat2\\step3");

s_ui.show_form(860,650);

s_ui.Form_Title("v2 第二步：选择要统计的文件");

s_net.ftp_list("robot6.funnyai.com","test",password,22,"/upload","callback_ftp_list");

