
function run_click(){
    var line=sys.ListBox_Text("list1");
    var head="java -jar E:\\happyli\\Jar\\funny_app\\funny_app.jar E:\\CloudStation\\Robot5\\GitHub\\funny_app_java\\funny_app\\";
    sys.Run_Cmd(head+line);
}


sys.Button_Init("b1_1","运行",100,10,200,30,"run_click","");

sys.ListBox_Init("list1",100,50,500,200);

sys.ListBox_From_File("list1",sys.App_Path()+"\\config\\funny_app.txt");


sys.TextBox_Init("txt_error","错误信息：",100,300,500,200);


sys.Show_Form(800,600);

sys.Form_Title("FunnyApp.jar Test");







