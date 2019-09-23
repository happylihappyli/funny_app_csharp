

function index_create(data){
    s_index.Create_Start("D:\\Net\\Web\\index",true);
    s_index.Add_Document("1","你好中国");
    s_index.Add_Document("2","中国社会发展");
    s_index.Create_End();
}



function index_add(data){
    s_index.Create_Start("D:\\Net\\Web\\index",false);
    s_index.Add_Document("3","发展中国家");
    s_index.Create_End();
}



function search(data){
    var seg=s_index.Seg(s_ui.text_read("txt1"));
    var result=s_index.Search("D:\\Net\\Web\\index",seg);
    
    s_xml.init(result,"doc1");
    
    var count=s_xml.count("doc1","/data/item");
    
    var html="";
    for (var i=1;i<=count;i++){
        var id=s_xml.read("doc1","/data/item[position()="+i+"]/id");
        var content=s_xml.read("doc1","/data/item[position()="+i+"]/content");
        html+="id="+id+"<br>"+content+"<hr>";
    }
    s_ui.Web_Content("web",html);
}

function test(data){
    var file1="E:\\CloudStation\\[持续的幸福flourish] 马丁·塞利格曼 著.pdf";
    var file2="E:\\test.pdf";
    var size=s_file.Size(file1);
    var read_size=10240;
    for (var i=0;i<size;i+=read_size){
        var base64=s_file.Bin_Read(file1,i,read_size);
        s_file.Bin_Write(file2,i,base64);
    }
}

//s_index.Init_Seg("D:\\Funny\\FunnyAI\\Data\\Dic\\Segmentation","callback_init");
s_ui.text_init("txt1","输入信息",10,10,200,30);

s_ui.button_init("b2","Test",250,50,200,30,"test","");


s_ui.Web_Init("web",10,100,700,500);

s_ui.Show_Form(800,680);
s_ui.Form_Title("Test Read Write");

