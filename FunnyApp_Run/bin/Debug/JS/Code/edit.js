
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
    
    var id=s_sys.Value_Read("ID");
    s_ui.Text_Set("txt1",id);
    
    var file=Path_Data+"\\"+id+".txt";
    var content=s_file.read(file);
    s_ui.Text_Set("txt2",content);
}

function save_click(data){
    var ID=s_ui.Text_Read("txt1");
    s_file.Ini_Save("D:\\Net\\Web\\main.ini","Code","max_id",ID);
    s_sys.Value_Save("ID",ID);
    s_sys.Value_Save("Content",s_ui.Text_Read("txt2"));
    s_ui.Close();
}


s_ui.TextBox_Init("txt2","",10,100,600,300);
s_ui.Text_Font_Size("txt2",18);
s_ui.Control_Dock("txt2","fill");

s_ui.Panel_Init("panel1",0,0,500,25,"top");

s_ui.Text_Init("txt1","1",10,30,200,30);
s_ui.Text_Read_Only("txt1",1);

s_ui.Button_Init("b_save","保存",250,30,200,30,"save_click","");


s_ui.Panel_Add("panel1","b_save","left");
s_ui.Panel_Add("panel1","txt1","left");



s_ui.Show_Form(700,600);
s_ui.Form_Title("新建文件");

init("");



