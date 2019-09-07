
function run_click(){
    var line=sys.ListBox_Text("list1");
    var strSplit=line.split("|");
    var head="java -jar E:\\happyli\\Jar\\funny_app\\funny_app.jar E:\\CloudStation\\Robot5\\GitHub\\funny_app_java\\funny_app\\js\\";
    var url=head+strSplit[0];
    //sys.Msg(url);
    sys.Run_Cmd(url);
    if (strSplit.length>1){
        sys.Run_JS_Out(strSplit[1]);
    }
}

function fold_open(data){
    sys.Open_Fold(sys.App_Path()+"\\config\\funny_app.txt");
}

sys.Button_Init("b1_1","运行",100,10,200,30,"run_click","");
sys.Button_Init("b1_1","文件",300,10,200,30,"fold_open","");

sys.ListBox_Init("list1",100,50,500,200);

sys.ListBox_From_File("list1",sys.App_Path()+"\\config\\funny_app.txt");


sys.TextBox_Init("txt_error","错误信息：",100,300,500,200);


sys.Show_Form(800,600);

sys.Form_Title("FunnyApp.jar Test");





