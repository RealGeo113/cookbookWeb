// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
let instructionCounter = 1;
let ingredientCounter = 1;

function DateNow() {
    let date = new Date();
    let yyyy = date.getFullYear();
    let dd = date.getDate();
    let mm = date.getMonth() + 1;

    if (dd < 10) dd = "0" + dd;

    if (mm < 10) mm = "0" + mm;

    let currentDay = yyyy + "." + mm + "." + dd;

    let hours = date.getHours();
    let minutes = date.getMinutes();
    let seconds = date.getSeconds();

    if (hours < 10) hours = "0" + hours;

    if (minutes < 10) minutes = "0" + minutes;

    if (seconds < 10) seconds = "0" + seconds;

    return currentDay + " " + hours + ":" + minutes + ":" + seconds;
}

function addInstruction() {

    let outerDiv = document.createElement('div');
    outerDiv.classList.add(...['instruction', 'row', 'container'])
    outerDiv.id = 'instruction-container-' + instructionCounter; 

    if (instructionCounter == 1) {
        let hr = document.createElement('hr');
        hr.classList.add(...['col-12']);
        outerDiv.appendChild(hr);
    }

    let removeDiv = document.createElement('div');
    removeDiv.classList.add(...['col-12', 'd-flex', 'justify-content-end']);

    let removeButton = document.createElement('button');
    removeButton.classList.add(...['btn', 'btn-danger']);
    removeButton.textContent = 'Remove';
    removeButton.type = 'button';

    removeButton.addEventListener('click', function (e) {
        let id = outerDiv.id.split("-")[2];

        document.querySelectorAll('.instruction').forEach(function (item) {
            let otherId = item.id.split("-")[2];
            let textarea = item.querySelector('textarea');
            let image = item.querySelector('.instruction-image');
            let input = item.querySelector('.custom-file-input');

            if (id < otherId) {
                otherId -= 1;
                item.id = 'instruction-container-' + otherId;
                textarea.name = "instructions[" + otherId + "].Content";
                image.id = "instruction-" + otherId + '-image';
                input.id = 'instruction-' + otherId;
                input.name = 'instructions[' + otherId + '].Image';
            }

        });
        instructionCounter -= 1;
        outerDiv.remove();
    });

    removeDiv.appendChild(removeButton);
    outerDiv.appendChild(removeDiv);

    let inputDiv = document.createElement('div');
    inputDiv.classList.add(...['col-md-6', 'form-group']);

    let labelContent = document.createElement('label');
    labelContent.classList.add(...['control-label']);
    labelContent.textContent = "Content:";

    let inputContent = document.createElement('textarea');
    inputContent.classList.add(...['form-control', 'instruction-content']);
    inputContent.name = 'instructions[' + instructionCounter + '].Content';

    inputDiv.appendChild(labelContent);
    inputDiv.appendChild(inputContent);

    let fileDiv = document.createElement('div');
    fileDiv.classList.add(...['custom-file']);

    let labelFile1 = document.createElement('label');
    labelFile1.classList.add(...['custom-file-label']);
    labelFile1.for = 'instruction-' + instructionCounter;
    labelFile1.textContent = 'Choose image...';

    let inputFile = document.createElement('input');
    inputFile.type = 'file';
    inputFile.classList.add(...['custom-file-input']);
    inputFile.id = 'instruction-' + instructionCounter;
    inputFile.name = 'instructions[' + instructionCounter + '].Image';

    inputFile.addEventListener('change', function (e) {
        var fileName = this.files[0].name;
        var nextSibling = e.target.nextElementSibling;
        nextSibling.innerText = fileName;

        readURL(this);
    });

    fileDiv.appendChild(inputFile);
    fileDiv.appendChild(labelFile1);

    let formDiv = document.createElement('div');
    formDiv.classList.add(...['form-group']);

    let labelFile2 = document.createElement('label');
    labelFile2.classList.add(...['control-label']);
    labelFile2.textContent = 'Image:';
    formDiv.appendChild(labelFile2);
    formDiv.appendChild(fileDiv);

    let imageDiv = document.createElement('div');
    imageDiv.classList.add(...['instruction-image-container']);

    let image = document.createElement('img');
    image.src = '/Images/placeholder-image.png';
    image.alt = 'Instruction Image';
    image.id = 'instruction-' + instructionCounter + '-image';
    image.classList.add(...['instruction-image']);

    imageDiv.appendChild(image);

    let imageContainerDiv = document.createElement('div');
    imageContainerDiv.classList.add(...['col-md-6', 'image-form']);

    imageContainerDiv.appendChild(imageDiv);
    imageContainerDiv.appendChild(formDiv);

    outerDiv.appendChild(inputDiv);
    outerDiv.appendChild(imageContainerDiv);

    let hr = document.createElement('hr');
    hr.classList.add(...['col-12']);
    outerDiv.appendChild(hr);

    instructionsContainer.appendChild(outerDiv);
    instructionCounter++;
}

function addIngredient() {

    let outerDiv = document.createElement('div');
    outerDiv.classList.add(...['ingredient', 'row', 'container']);
    outerDiv.id = 'ingredient-container-' + ingredientCounter;
    if (ingredientCounter == 1) {
        let hr = document.createElement('hr');
        hr.classList.add(...['col-12']);
        outerDiv.appendChild(hr);
    }

    let removeDiv = document.createElement('div');
    removeDiv.classList.add(...['col-12', 'd-flex', 'justify-content-end']);

    let removeButton = document.createElement('button');
    removeButton.classList.add(...['btn', 'btn-danger']);
    removeButton.textContent = 'Remove';
    removeButton.type = 'button';

    removeButton.addEventListener('click', function (e) {
        let id = outerDiv.id.split("-")[2];

        document.querySelectorAll('.ingredient').forEach(function (item) {
            let otherId = item.id.split("-")[2];
            let name = item.querySelector('.ingredient-name');
            let amount = item.querySelector('.ingredient-amount');
            let unit = item.querySelector('.ingredient-unit');

            if (id < otherId) {
                otherId -= 1;
                item.id = 'ingredient-container-' + otherId;
                name.name = 'ingredients[' + otherId + '].Name';
                amount.name = 'ingredients[' + otherId + '].Amount';
                unit.name = 'ingredients[' + otherId + '].Unit';
            }

        });
        ingredientCounter -= 1;
        outerDiv.remove();
    });

    removeDiv.appendChild(removeButton);
    outerDiv.appendChild(removeDiv);

    let labelName = document.createElement('label');
    labelName.classList.add(...['control-label']);
    let innerDiv1 = document.createElement('div');
    innerDiv1.classList.add(...['col-lg-4', 'form-group']);
    labelName.textContent = "Name:"
    let inputName = document.createElement('input');
    inputName.classList.add(...['form-control', 'ingredient-name']);
    inputName.name = 'ingredients[' + ingredientCounter + '].Name';
    inputName.required = true;

    innerDiv1.appendChild(labelName);
    innerDiv1.appendChild(inputName);
    outerDiv.appendChild(innerDiv1);

    let labelAmount = document.createElement('label');
    labelAmount.classList.add(...['control-label']);
    let innerDiv2 = document.createElement('div');
    innerDiv2.classList.add(...['col-lg-4', 'form-group']);
    labelAmount.textContent = "Amount:"
    let inputAmount = document.createElement('input');
    inputAmount.classList.add(...['form-control', 'ingredient-amount']);
    inputAmount.name = 'ingredients[' + ingredientCounter + '].Amount';
    inputAmount.type = 'number';
    inputAmount.min = 0;
    inputAmount.max = 999;
    inputAmount.required = true;

    innerDiv2.appendChild(labelAmount);
    innerDiv2.appendChild(inputAmount);
    outerDiv.appendChild(innerDiv2);

    let labelUnit = document.createElement('label');
    labelUnit.classList.add(...['control-label']);

    let innerDiv3 = document.createElement('div');
    innerDiv3.classList.add(...['col-lg-4', 'form-group']);
    labelUnit.textContent = "Unit:"
    let inputUnit = document.createElement('input');
    inputUnit.classList.add(...['form-control', 'ingredient-unit']);
    inputUnit.name = 'ingredients[' + ingredientCounter + '].Unit';
    inputUnit.required = true;

    innerDiv3.appendChild(labelUnit);
    innerDiv3.appendChild(inputUnit);
    outerDiv.appendChild(innerDiv3);

    let hr = document.createElement('hr');
    hr.classList.add(...['col-12']);
    outerDiv.appendChild(hr);

    ingredientsContainer.appendChild(outerDiv);
    ingredientCounter++;
}

document.querySelectorAll('.custom-file-input').forEach(function (item) {
    item.addEventListener('change', function (e) {
        let fileName = this.files[0].name;
        let nextSibling = e.target.nextElementSibling;
        nextSibling.innerText = fileName;

        readURL(this);
    });
});

document.querySelectorAll('.btn-remove-ingredient').forEach(function (item) {
    item.addEventListener('click', function (e) {
        let parent = item.closest('.ingredient');
        let id = parent.id.split("-")[2];

        document.querySelectorAll('.ingredient').forEach(function (item) {
            let otherId = item.id.split("-")[2];
            let name = item.querySelector('.ingredient-name');
            let amount = item.querySelector('.ingredient-amount');
            let unit = item.querySelector('.ingredient-unit');

            if (id < otherId) {
                otherId -= 1;
                item.id = 'ingredient-container-' + otherId;
                name.name = 'ingredients[' + otherId + '].Name';
                amount.name = 'ingredients[' + otherId + '].Amount';
                unit.name = 'ingredients[' + otherId + '].Unit';
            }

        });
        ingredientCounter -= 1;
        parent.remove();
    });
});

document.querySelectorAll('.btn-remove-instruction').forEach(function (item) {
    item.addEventListener('click', function (e) {
        let parent = item.closest('.instruction');
        let id = parent.id.split("-")[2];

        document.querySelectorAll('.instruction').forEach(function (item) {
            let otherId = item.id.split("-")[2];
            let textarea = item.querySelector('textarea');
            let image = item.querySelector('.instruction-image');
            let input = item.querySelector('.custom-file-input');

            if (id < otherId) {
                otherId -= 1;
                item.id = 'ingredient-contaiener-' + otherId;
                textarea.name = "instructions[" + otherId + "].Content";
                image.id = "instruction-" + otherId + '-image';
                input.id = 'instruction-' + otherId;
                input.name = 'instructions[' + otherId + '].Image';
            }

        });
        instructionCounter -= 1;
        parent.remove();
    });
});

function readURL(input) {
    let imageId = input.id + '-image';
    console.log(imageId);
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#' + imageId).attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

function remove(element) {
    console.log(element);
}