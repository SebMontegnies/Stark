$(function () {

	$('#file').change(function () {
		$('#file-name').text($('#file')[0].files[0].name);
		file = $('#file')[0].files[0];
		if (file !== null) {
			$('#file-loader').css({ 'display': 'inline-block' });
			var imageBase64;
			var reader = new FileReader();
			reader.readAsDataURL(file);
			reader.onload = function () {
				imageBase64 = reader.result;
			};
			reader.onerror = function (error) {
				console.log('Error: ', error);
			};
			$.ajax({
				type: "POST",
				url: "/api/File",
				data: { data: imageBase64 },
				success: function (result) {
					$('.question.file').hide();
					var documentName = '<span>' + file.name + '</span>';
					var symptom = '<span>' + result + '</span>';
					$('#files-list').append('<img src="' + imageBase64 + '" width="320" />');
					$('#files-list').append('<span class="symptom">The system identifies this pathology : <strong>' + symptom + '<strong></span></div><br />');
				}
			});
		}
	});

});