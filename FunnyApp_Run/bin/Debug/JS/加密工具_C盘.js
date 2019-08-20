
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
    var txt_file=sys.Get_Text("txt_file_public");
    var txt_file2=txt_file.replace(".pub",".pem.pub");
    var param="-f "+txt_file+" -e -m PKCS8";// > "+txt_file2;
    
    show_error(file_key);
    show_error(param);
    
    var result=sys.Run_App_Return(file_key,param);
    sys.File_Save(txt_file2,result);
    
    show_error(result);
    
    //sys.Run_App("explorer.exe","C:\\Net\\Web\\public\\");
}

function file_pem_private(){
    var file_key="C:\\Windows\\System32\\OpenSSH\\ssh-keygen.exe";
    var txt_file="C:\\Net\\Web\\id_rsa";
    var txt_file2="C:\\Net\\Web\\id_rsa.pem";//txt_file.replace(".pub",".pem.pub");
    var param="pkcs8 -topk8 -inform PEM -outform DER -in "+txt_file+" -nocrypt > "+txt_file2;
    //pkcs8 -topk8 -inform PEM -outform DER -in private_key_file  -nocrypt > pkcs8_key
    show_error(file_key);
    show_error(param);
    
    var result=sys.Run_App_Return(file_key,param);
    sys.File_Save(txt_file2,result);
    
    show_error(result);
    
    //sys.Run_App("explorer.exe","C:\\Net\\Web\\public\\");
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
    var strFile=sys.Get_Text("txt_file_private");
    var strLine=sys.decrypt_private_key(strFile,strMsg);
    sys.Show_Text("txt_input2",strLine);
}

function key_click(data){
    sys.Msg("一会打开程序，请按两个回车，自动关闭窗口");
    sys.Run_App("C:\\Windows\\System32\\OpenSSH\\ssh-keygen.exe"," -m PEM -t rsa -b 2048 -C test -f C:/Net/Web/id_rsa");
    sys.Show_Text("txt1","把这个文件 C:\\Net\\Web\\id_rsa.pub 发到服务器，让管理员设置");
}



sys.Add_Button("b1","创建key",650,10,100,30,"key_click","0");

sys.Add_Label("lb_file","文件：",10,10);

sys.Add_Text("txt_file_public","C:/Net/Web/public/id_rsa_happyli.pub",100,10,500,30);
sys.Add_Button("b2_1","选择文件",100,50,200,30,"file_open","");


sys.Add_Button("b2_2","转为pem格式",300,50,200,30,"file_pem","");
sys.Add_Button("b2_3","转为pem格式(p)",500,50,200,30,"file_pem_private","");



sys.Add_Label("lb_upload","加密公钥文件：",10,110);

sys.Add_Text("txt_file_public_pem","C:/Net/Web/public/pem/id_rsa_happyli.pem.pub",100,110,500,30);


sys.Add_Label("lb_upload","解密私钥文件：",10,150);
sys.Add_Text("txt_file_private","C:/Net/Web/id_rsa",100,150,500,30);


sys.Add_Text("txt_input","hello，你好",100,200,500,30);
sys.Add_Button("b3_1","加密",100,250,100,30,"encrypt_click","");
sys.Add_Button("b3_1","复制",300,250,100,30,"copy_click","");
sys.Add_Button("b3_1","解密",500,250,100,30,"decrypt_click","");
sys.Add_Text("txt_input2","",100,300,500,30);



sys.Add_Text_Multi("txt_error","错误信息：",100,350,500,200);



sys.Show_Form(800,600);

sys.Form_Title("加密工具");







