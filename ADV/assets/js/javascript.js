function ucwords(str){
	var kata = str.split(" ");
	var i = 0; 
	var kalimat = '';
	var f;
	for(i;i<kata.length;i++){
		f = kata[i].charAt(0).toUpperCase();
		kalimat += f+kata[i].substr(1)+" ";
	}
	return kalimat;
}

function validasi(delimiter,elementId){
	var formElement;
	var jumlahArray;
	var i;
	var formName = "";
	
	formElement = elementId.split(delimiter);
	
	for(i=0;i<formElement.length;i++){
		if(trim(document.getElementById(formElement[i]).value) == ""){
			formName = formElement[i];
			break;
		}
	}
	
	if(formName != ""){
		alert('Maaf, silahkan isi form '+ucwords(formName));	
		document.getElementById(formName).focus();
		return false;
	}else{
		return true;
	}
}

function trim(str){
    return str.replace(/^\s+|\s+$/g,'');
}

function isNumeric ( evt ) {
    var charCode = ( evt.which ) ? evt.which : event.keyCode;
    if ( charCode > 31 && (charCode < 48 || charCode > 57) ) return false;
    return true;
}