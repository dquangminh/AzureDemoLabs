var input = document.querySelector('#upload');
input.addEventListener('change', handleUploadImage);
function handleUploadImage(e) {
    var fr = new FileReader();
    fr.onload = function(event) {
        const img = new Image();
        img.src = event.target['result'];
        img.onload = function() {
            loadImage(img);
        };
    };
    fr.readAsDataURL(e.target['files'][0]);
}

function loadImage(img)
{
    EXIF.getData(img, function() {
        const orientation = EXIF.getTag(this, 'Orientation');
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');
        canvas.width = img.width;
        canvas.height = img.height;
        
        // if (orientation >= 5) {
        //     canvas.width = img.height;
        //     canvas.height = img.width;
        // }
        // switch (orientation) {
        //     case 2:
        //         ctx.transform(-1, 0, 0, 1, img.width, 0);
        //         break;
        //     case 3:
        //         ctx.transform(-1, 0, 0, -1, img.width, img.height);
        //         break;
        //     case 4:
        //         ctx.transform(1, 0, 0, -1, 0, img.height);
        //         break;
        //     case 5:
        //         ctx.transform(0, 1, 1, 0, 0, 0);
        //         break;
        //     case 6:
        //         alert(1);
        //         ctx.transform(0, 1, -1, 0, img.height, 0);
        //         break;
        //     case 7:
        //         ctx.transform(0, -1, -1, 0, img.height, img.width);
        //         break;
        //     case 8:
        //         ctx.transform(0, -1, 1, 0, 0, img.width);
        //         break;
        // }
        ctx.drawImage(img, 0, 0);
        img = null;
        sourceImg = null;
        download(canvas.toDataURL('image/jpeg', 1.0), 'abc.jpeg');
    });
}

function download(dataurl, filename) {
    var a = document.createElement("a");
    a.href = dataurl;
    a.setAttribute("download", filename);
    a.click();
  }