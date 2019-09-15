
var userName="none";
var log_msg="";

function save_check_click(data){

    var path1=s_ui.Text_Read("path1");
    var path2=s_ui.Text_Read("path2");
    var path3=s_ui.Text_Read("path3");
    
    s_file.Ini_Save("D:\\Net\\Web\\main.ini","Code","Path_Data",path1);
    s_file.Ini_Save("D:\\Net\\Web\\main.ini","Code","Path_Index",path2);
    s_file.Ini_Save("D:\\Net\\Web\\main.ini","Code","Path_Seg",path3);
    s_ui.Msg("保存成功！");
}

var Path_Data =s_file.Ini_Read("D:\\Net\\Web\\main.ini","Code","Path_Data");
var Path_Index=s_file.Ini_Read("D:\\Net\\Web\\main.ini","Code","Path_Index");
var Path_Seg  =s_file.Ini_Read("D:\\Net\\Web\\main.ini","Code","Path_Seg");


s_ui.Label_Init("lb1","设置代码目录和索引目录",10,30);

s_ui.Label_Init("lb2","代码目录:",10,100);
s_ui.Text_Init("path1",Path_Data,150,100,300,30);
s_ui.Label_Init("lb3","索引目录:",10,150);
s_ui.Text_Init("path2",Path_Index,150,150,300,30);
s_ui.Label_Init("lb4","词库目录:",10,200);
s_ui.Text_Init("path3",Path_Seg,150,200,300,30);

s_ui.Button_Init("b1_save","保存",150,300,100,30,"save_check_click","");


//其他属性
s_ui.Acception_Button("b1_save");
s_ui.Show_Form(560,380);
s_ui.Form_Title("目录设置");
