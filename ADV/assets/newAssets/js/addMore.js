// Add more question

const cardContainer = document.getElementById("card-container");
const addMoreBtn = document.getElementById("newsectionbtn");

let cardCount = 1;

addMoreBtn.addEventListener("click", function () {
  const newCard = document.createElement("div");
  newCard.classList.add("card", "radius-10", "mb-4");
  newCard.id = "sectionQuestion" + cardCount;
  newCard.innerHTML = `
    <div class="card-body order-actions">
      <div class="row mb-3">
            <div class="col-12 col-lg-6">
            <h6 class="mb-3 text-black">Question ${cardCount + 1}</h6>
            <textarea class="form-control bg-light-secondary" id="inputQuestionArea" placeholder="Input Question..." rows="1"></textarea>
        </div>
        <div class="col-12 col-lg-6">
            <label for="formFile" class="form-label">Image</label>
            <input class="form-control" type="file" id="formFile">
        </div>
      </div>

      <hr>

        <div class="card radius-10 mb-2">
            <div class="card-body">
                <div class="form-check d-flex align-items-center mb-2">
                    <input class="form-check-input me-2" type="radio" name="question${
                      cardCount + 1
                    }"
                        id="question${cardCount + 1}1">
                    <input type="text" class="form-control" for="question${
                      cardCount + 1
                    }1"
                        placeholder="Input Answer...">
                </div>
                <div class="">
                    <label for="formFile" class="form-label">Image</label>
                    <input class="form-control" type="file" id="formFile">
                </div>
            </div>
        </div>
        <div class="card radius-10 mb-2">
            <div class="card-body">
                <div class="form-check d-flex align-items-center mb-2">
                    <input class="form-check-input me-2" type="radio" name="question${
                      cardCount + 1
                    }"
                        id="question${cardCount + 1}2">
                    <input type="text" class="form-control" for="question${
                      cardCount + 1
                    }2"
                        placeholder="Input Answer...">
                </div>
                <div class="">
                    <label for="formFile" class="form-label">Image</label>
                    <input class="form-control" type="file" id="formFile">
                </div>
            </div>
        </div>
        <div class="card radius-10 mb-2">
            <div class="card-body">
                <div class="form-check d-flex align-items-center mb-2">
                    <input class="form-check-input me-2" type="radio" name="question${
                      cardCount + 1
                    }"
                        id="question${cardCount + 1}3">
                    <input type="text" class="form-control" for="question${
                      cardCount + 1
                    }3"
                        placeholder="Input Answer...">
                </div>
                <div class="">
                    <label for="formFile" class="form-label">Image</label>
                    <input class="form-control" type="file" id="formFile">
                </div>
            </div>
        </div>
        <div class="card radius-10 mb-2">
            <div class="card-body">
                <div class="form-check d-flex align-items-center mb-2">
                    <input class="form-check-input me-2" type="radio" name="question${
                      cardCount + 1
                    }"
                        id="question${cardCount + 1}4">
                    <input type="text" class="form-control" for="question${
                      cardCount + 1
                    }4"
                        placeholder="Input Answer...">
                </div>
                <div class="">
                    <label for="formFile" class="form-label">Image</label>
                    <input class="form-control" type="file" id="formFile">
                </div>
            </div>
        </div>

        <div class="d-flex justify-content-end">
            <div class="mt-2">
                <label class="form-label" for="">Delete</label>
                <a type="button"
                    class="text-danger bg-light-danger border-0 ms-2"><i
                        class="bx bxs-trash" data-bs-toggle="modal" data-bs-target="#modalDelete"></i></a>
            </div>
        </div>
    </div>
  `;
  cardContainer.appendChild(newCard);
  cardCount++;
});
