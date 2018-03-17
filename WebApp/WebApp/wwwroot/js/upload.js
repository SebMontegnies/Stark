function getBase64(file) {
	var reader = new FileReader();
	reader.readAsDataURL(file);
	reader.onload = function () {
		return reader.result;
	};
	reader.onerror = function (error) {
		console.log('Error: ', error);
	};
}

$(function () {

	$('#file').change(function () {
		$('#file-name').text($('#file')[0].files[0].name);
		file = $('#file')[0].files[0];
		if (file !== null) {
			$('#file-loader').css({ 'display': 'inline-block' });
	
			 
			$.ajax({
				type: "GET",
				url: "http://localhost:2921/api/File",
				data: { bytes: getBase64(file) },
				success: function (result) {
					console.log(result);
					var documentName = '<span>' + file.name + '</span>';
					var symptomResult = 'Souffle au coeur';
					var symptom = '<span>' + symptomResult + '</span>'; //result.symptom
					$('#files-list').append('<div class="file">' + symptom + '<br /><span class="file-name">' + documentName + '</span></div>');
				}
			});
		}
	});

});