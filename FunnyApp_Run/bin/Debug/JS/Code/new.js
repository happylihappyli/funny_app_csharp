
var Path_Data ="";
var Path_Index="";
var Path_Seg="";
var max_id=1;


function init(data){
    
    Path_Data =s_file.Ini_Read("D:\\Net\\Web\\main.ini","Code","Path_Data");
    Path_Index=s_file.Ini_Read("D:\\Net\\Web\\main.ini","Code","Path_Index");
    Path_Seg  =s_file.Ini_Read("D:\\Net\\Web\\main.ini","Code","Path_Seg");
    max_id  =s_file.Ini_Read("D:\\Net\\Web\\main.ini","Code","max_id");
    if (Path_Data=="" || Path_Index=="" || Path_Seg==""){
        set_click("");
    }
    if (max_id==""){
        max_id=0;
    }else{
        max_id=parseInt(max_id);
    }
    max_id+=1;
    s_ui.text_set("txt1",max_id);
}

function save_click(data){
    var ID=s_ui.text_read("txt1");
    s_file.Ini_Save("D:\\Net\\Web\\main.ini","Code","max_id",ID);
    s_sys.value_save("ID",ID);
    s_sys.value_save("Content",s_ui.text_read("txt2"));
    s_ui.close();
}


s_ui.textbox_init("txt2","",10,100,600,300);
s_ui.text_font_size("txt2",18);
s_ui.Control_Dock("txt2","fill");

s_ui.panel_init("panel1",0,0,500,25,"top");

s_ui.text_init("txt1","1",10,30,200,30);
s_ui.text_read_only("txt1",1);

s_ui.button_init("b_save","保存",250,30,200,30,"save_click","");


s_ui.panel_add("panel1","b_save","left");
s_ui.panel_add("panel1","txt1","left");



s_ui.Show_Form(700,600);
s_ui.Form_Title("新建文件");

init("");



