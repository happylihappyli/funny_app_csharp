
var log_error="";
function show_error(data){
    log_error+=data+"\r\n";
    sys.Show_Text("txt_error",log_error);
}


function file_open(){
    var strLine=sys.File_Open();
    sys.Show_Text("txt_file_public",strLine);
}

function file_pem(){
    var file_key="C:\\Windows\\System32\\OpenSSH\\ssh-keygen.exe";
    var txt_file=sys.Get_Text("txt_file");
    var txt_file2=txt_file.replace(".pub",".pem.pub");
    var param="-f "+txt_file+" -e -m PKCS8";// > "+txt_file2;
    
    show_error(file_key);
    show_error(param);
    
    var result=sys.Run_App(file_key,param);
    sys.File_Save(txt_file2,result);
    
    show_error(result);
    
    sys.Run_App("explorer.exe","D:\\Net\\Web\\public\\");
    
}

function encrypt_click(data){
    
    var file=sys.Get_Text("txt_file_public_pem");
    var strMsg=sys.Get_Text("txt_input");
    var strLine=sys.encrypt_public_key(file,strMsg);
    sys.Show_Text("txt_input2",strLine);
    
}

function copy_click(){
    
    var strMsg=sys.Get_Text("txt_input2");
    sys.Show_Text("txt_input",strMsg);
    sys.Show_Text("txt_input2","");
}


function decrypt_click(){
    var strMsg=sys.Get_Text("txt_input");
    var strLine=sys.decrypt_private_key("D:/Net/Web/id_rsa",strMsg);
    sys.Show_Text("txt_input2",strLine);
}



sys.Add_Label("lb_upload","文件：",10,10);

sys.Add_Text("txt_file_public","D:/Net/Web/public/id_rsa_happyli.pub",100,10,500,30);
sys.Add_Button("b2_1","选择文件",100,50,200,30,"file_open","");


sys.Add_Button("b2_2","转为pem格式",300,50,200,30,"file_pem","");

sys.Add_Text("txt_file_public_pem","D:/Net/Web/public/pem/id_rsa_happyli.pem.pub",100,110,500,30);




sys.Add_Text("txt_input","hello，你好",100,150,500,30);
sys.Add_Button("b3_1","加密",100,200,100,30,"encrypt_click","");
sys.Add_Button("b3_1","复制",300,200,100,30,"copy_click","");
sys.Add_Button("b3_1","解密",500,200,100,30,"decrypt_click","");
sys.Add_Text("txt_input2","",100,250,500,30);



sys.Add_Text_Multi("txt_error","错误信息：",100,300,500,200);



sys.Show_Form(800,600);

sys.Form_Title("加密工具");







