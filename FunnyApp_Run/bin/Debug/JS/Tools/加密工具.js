
var log_error="";
function show_error(data){
    log_error+=data+"\r\n";
    s_ui.Text_Set("txt_error",log_error);
}


function file_open(){
    var strLine=sys.File_Open();
    s_ui.Text_Set("txt_file_public",strLine);
}

function file_pem(){
    var file_key="C:\\Windows\\System32\\OpenSSH\\ssh-keygen.exe";
    var txt_file=s_ui.Text_Read("txt_file_public");
    var txt_file2=txt_file.replace(".pub",".pem.pub");
    var param="-f "+txt_file+" -e -m PKCS8";// > "+txt_file2;
    
    show_error(file_key);
    show_error(param);
    
    var result=s_ui.Run_App_Return(file_key,param);
    sys.File_Save(txt_file2,result);
    
    show_error(result);
    
    //s_ui.Run_App("explorer.exe","D:\\Net\\Web\\public\\");
}

function file_pem_private(){
    var file_key="C:\\Windows\\System32\\OpenSSH\\ssh-keygen.exe";
    var txt_file="D:\\Net\\Web\\id_rsa";
    var txt_file2="D:\\Net\\Web\\id_rsa.pem";//txt_file.replace(".pub",".pem.pub");
    var param="pkcs8 -topk8 -inform PEM -outform DER -in "+txt_file+" -nocrypt > "+txt_file2;
    //pkcs8 -topk8 -inform PEM -outform DER -in private_key_file  -nocrypt > pkcs8_key
    show_error(file_key);
    show_error(param);
    
    var result=s_ui.Run_App_Return(file_key,param);
    sys.File_Save(txt_file2,result);
    
    show_error(result);
    
    //s_ui.Run_App("explorer.exe","D:\\Net\\Web\\public\\");
}


function encrypt_click(data){
    
    var file=s_ui.Text_Read("txt_file_public_pem");
    var strMsg=s_ui.Text_Read("txt_input");
    var strLine=sys.encrypt_public_key(file,strMsg);
    s_ui.Text_Set("txt_input2",strLine);
    
}

function copy_click(){
    
    var strMsg=s_ui.Text_Read("txt_input2");
    s_ui.Text_Set("txt_input",strMsg);
    s_ui.Text_Set("txt_input2","");
}


function decrypt_click(){
    var strMsg=s_ui.Text_Read("txt_input");
    var strFile=s_ui.Text_Read("txt_file_private");
    var strLine=sys.decrypt_private_key(strFile,strMsg);
    s_ui.Text_Set("txt_input2",strLine);
}

function key_click(data){
    s_ui.Msg("一会打开程序，请按两个回车，自动关闭窗口");
    s_ui.Run_App("C:\\Windows\\System32\\OpenSSH\\ssh-keygen.exe"," -m PEM -t rsa -b 2048 -C test -f D:/Net/Web/id_rsa");
    s_ui.Text_Set("txt1","把这个文件 D:\\Net\\Web\\id_rsa.pub 发到服务器，让管理员设置");
}



s_ui.Button_Init("b1","创建key",650,10,100,30,"key_click","0");

s_ui.Label_Init("lb_file","文件：",10,10);

s_ui.Text_Init("txt_file_public","D:/Net/Web/public/id_rsa_happyli.pub",100,10,500,30);
s_ui.Button_Init("b2_1","选择文件",100,50,200,30,"file_open","");


s_ui.Button_Init("b2_2","转为pem格式",300,50,200,30,"file_pem","");
s_ui.Button_Init("b2_3","转为pem格式(p)",500,50,200,30,"file_pem_private","");



s_ui.Text_Init("txt_file_public_pem","D:/Net/Web/public/pem/id_rsa_happyli.pem.pub",100,110,500,30);


s_ui.Label_Init("lb_upload","解密私钥文件：",10,150);





s_ui.Text_Init("txt_file_private","D:/Net/Web/id_rsa",100,150,500,30);


s_ui.Text_Init("txt_input","hello，你好",100,200,500,30);


s_ui.Button_Init("b3_1","加密",100,250,100,30,"encrypt_click","");
s_ui.Button_Init("b3_2","复制",300,250,100,30,"copy_click","");
s_ui.Button_Init("b3_3","解密",500,250,100,30,"decrypt_click","");

s_ui.Text_Init("txt_input2","",100,300,500,30);



s_ui.TextBox_Init("txt_error","错误信息：",100,350,500,200);



s_ui.Show_Form(800,600);

s_ui.Form_Title("加密工具");


