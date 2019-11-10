
var disk="D:";
var log_error="";
function show_error(data){
    log_error+=data+"\r\n";
    s_ui.text_set("txt_error",log_error);
}


function file_open(){
    var strLine=s_file.File_Open();
    s_ui.text_set("txt_file_public",strLine);
}

function file_pem(){
    var file_key="C:\\Windows\\System32\\OpenSSH\\ssh-keygen.exe";
    var txt_file=s_ui.text_read("txt_file_public");
    var txt_file2=txt_file.replace(".pub",".pem.pub");
    var param="-f "+txt_file+" -e -m PKCS8";// > "+txt_file2;
    
    show_error(file_key);
    show_error(param);
    
    var result=s_ui.Run_App_Return(file_key,param);
    s_file.File_Save(txt_file2,result);
    
    show_error(result);
    
    //s_ui.Run_App("explorer.exe","D:\\Net\\Web\\public\\");
}

function file_pem_private(){
    var file_key="C:\\Windows\\System32\\OpenSSH\\ssh-keygen.exe";
    var txt_file=disk+"\\Net\\Web\\id_rsa";
    var txt_file2=disk+"\\Net\\Web\\id_rsa.pem";//txt_file.replace(".pub",".pem.pub");
    var param="pkcs8 -topk8 -inform PEM -outform DER -in "+txt_file+" -nocrypt > "+txt_file2;
    //pkcs8 -topk8 -inform PEM -outform DER -in private_key_file  -nocrypt > pkcs8_key
    show_error(file_key);
    show_error(param);
    
    var result=s_ui.Run_App_Return(file_key,param);
    s_file.File_Save(txt_file2,result);
    
    show_error(result);
    
    //s_ui.Run_App("explorer.exe",disk+"\\Net\\Web\\public\\");
}


function encrypt_click(data){
    
    var file=s_ui.text_read("txt_file_public_pem");
    var strMsg=s_ui.text_read("txt_input");
    var strLine=s_string.encrypt_public_key(file,strMsg);
    s_ui.text_set("txt_input2",strLine);
    
}

function copy_click(){
    
    var strMsg=s_ui.text_read("txt_input2");
    s_ui.text_set("txt_input",strMsg);
    s_ui.text_set("txt_input2","");
}


function decrypt_click(){
    var strMsg=s_ui.text_read("txt_input");
    var strFile=s_ui.text_read("txt_file_private");
    var strLine=s_string.decrypt_private_key(strFile,strMsg);
    s_ui.text_set("txt_input2",strLine);
}

function key_click(data){
    s_ui.msg("一会打开程序，请按两个回车，自动关闭窗口");
    s_ui.Run_App("C:\\Windows\\System32\\OpenSSH\\ssh-keygen.exe"," -m PEM -t rsa -b 2048 -C test -f D:/Net/Web/id_rsa");
    s_ui.text_set("txt1","把这个文件 D:\\Net\\Web\\id_rsa.pub 发到服务器，让管理员设置");
}

function file_encrypt1(data){
    var file1=s_ui.text_read("txt_file1");
    var file2=s_ui.text_read("txt_file2");
    var file_pub=s_ui.text_read("txt_file_public_pem");
    
    var step=64;
    var count=s_file.Size(file1);
    s_file.delete(file2);
    for (var i=0;i<count;i+=step){
        var base64=s_file.Bin_Read(file1,i,step);
        s_string.encrypt_public_key(file_pub,base64);
        s_file.append(file2,base64+"\n");
    }
}



function file_encrypt2(data){
    var file1=s_ui.text_read("txt_file1");
    var file2=s_ui.text_read("txt_file2");
    var file_pub=s_ui.text_read("txt_file_public_pem");
    
    var key="test";
    var step=1024;
    var count=s_file.Size(file1);
    s_file.delete(file2);
    for (var i=0;i<count;i+=step){
        var base64=s_file.Bin_Read(file1,i,step);
        var encrypt=s_string.AES_Encrypt(base64,key);
        s_file.append(file2,encrypt+"\n");
    }
}

function callback_file_decrypt2(data){
    s_ui.msg("解密结束！");
}

function file_decrypt2(data){
    //var file1=s_sys.value_read("file1");// s_ui.text_read("txt_file2");
    //var file2=s_sys.value_read("file2");//s_ui.text_read("txt_file3");
    s_sys.value_save("file1",s_ui.text_read("txt_file2"));
    s_sys.value_save("file2",s_ui.text_read("txt_file3"));
    s_sys.call_thread("sub.decrypt.js","callback_file_decrypt2");
}

s_ui.label_init("lb_file","要加密的文件：",10,10);

s_ui.text_init("txt_file1","D:\\蜡笔小新_高清版 (263).flv",100,10,500,30);
s_ui.button_init("b2_1","选择文件",100,50,200,30,"file_open","");



s_ui.label_init("lb_public","公钥文件：",10,110);
s_ui.text_init("txt_file_public_pem",disk+"/Net/Web/public/pem/id_rsa_happyli.pem.pub",100,110,500,30);


s_ui.label_init("lb_private","解密私钥文件：",10,150);
s_ui.text_init("txt_file_private",disk+"/Net/Web/id_rsa",100,150,500,30);


s_ui.label_init("lb_output","输出文件：",10,200);
s_ui.text_init("txt_file2","D:\\蜡笔小新_高清版 (263)_encrypt.flv",100,200,500,30);


s_ui.label_init("lb_output2","输出文件2：",10,250);
s_ui.text_init("txt_file3","D:\\蜡笔小新_高清版 (263)_new.flv",100,250,500,30);


s_ui.button_init("b3_1","加密",100,300,100,30,"file_encrypt2","");
s_ui.button_init("b3_3","解密",500,300,100,30,"file_decrypt2","");


s_ui.textbox_init("txt_error","错误信息：",100,350,500,200);



s_ui.show_form(800,600);

s_ui.Form_Title("文件加密");


