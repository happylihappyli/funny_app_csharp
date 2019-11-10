
var project_name="Movie";
var userName="none";
var log_msg="";

[[[..\\data\\default.js]]]

function save_click(data){

    var path1=s_ui.text_read("path1");
    var path2=s_ui.text_read("path2");
    var path3=s_ui.text_read("path3");
    
    s_file.Ini_Save(disk+"\\Net\\Web\\main.ini",Movie,"Path_Data",path1);
    s_file.Ini_Save(disk+"\\Net\\Web\\main.ini",Movie,"Path_Index",path2);
    s_file.Ini_Save(disk+"\\Net\\Web\\main.ini",Movie,"Path_Seg",path3);
    
    
    s_ui.msg("保存成功！设置完毕要重启启动程序");
}

var Path_Data =s_file.Ini_Read(disk+"\\Net\\Web\\main.ini",Movie,"Path_Data");
var Path_Index=s_file.Ini_Read(disk+"\\Net\\Web\\main.ini",Movie,"Path_Index");
var Path_Seg  =s_file.Ini_Read(disk+"\\Net\\Web\\main.ini",Movie,"Path_Seg");


s_ui.label_init("lb1","设置代码目录和索引目录",10,30);

s_ui.label_init("lb2","存储目录:",10,100);
s_ui.text_init("path1",Path_Data,150,100,300,30);
s_ui.label_init("lb3","索引目录:",10,150);
s_ui.text_init("path2",Path_Index,150,150,300,30);
s_ui.label_init("lb4","词库目录:",10,200);
s_ui.text_init("path3",Path_Seg,150,200,300,30);

s_ui.button_init("b1_save","保存",150,300,100,30,"save_click","");


//其他属性
s_ui.button_default("b1_save");
s_ui.show_form(560,380);
s_ui.Form_Title("目录设置");
