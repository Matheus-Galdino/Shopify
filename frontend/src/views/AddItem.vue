<template>
  <div class="add-item-container">
    <h2>Add a new item</h2>

    <form @submit.prevent="addItem">
      <div class="input-group">
        <label for="name">Name</label>
        <input
          type="text"
          id="name"
          placeholder="Enter a name"
          autocomplete="off"
          v-model="name"
        />
      </div>

      <div class="input-group">
        <label for="note">Note (optional)</label>
        <textarea
          id="note"
          placeholder="Enter a note"
          v-model="note"
          rows="4"
        ></textarea>
      </div>

      <div class="input-group">
        <label for="image">Image (optional)</label>
        <input
          type="url"
          id="image"
          v-model="imageUrl"
          autocomplete="off"
          placeholder="Enter a url"
        />
      </div>

      <div class="input-group">
        <label for="category">Category</label>
        <select id="category" v-model="categoryId">
          <option value="-1" selected disabled>Select a category...</option>
          <option
            v-for="category in allCategories"
            :value="category.id"
            :key="category.id"
          >
            {{ category.name }}
          </option>
        </select>
      </div>

      <div class="buttons-container">
        <button @click="$emit('close')">Cancel</button>
        <button @click.prevent="addItem" class="save">Save</button>
      </div>
    </form>

    <footer>
      <input
        type="text"
        v-model="newCategoryName"
        placeholder="New category name"
      />
      <button @click="addCategory">Add category</button>
    </footer>
  </div>
</template>

<script>
import API from "@/API";

export default {
  name: "AddItem",
  props: {
    allCategories: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      name: "",
      note: "",
      imageUrl: "",
      categoryId: -1,
      newCategoryName: "",
    };
  },
  methods: {
    async addItem() {
      const error = this.validateFields();

      if (error) {
        this.$emit("show-notification", [error.message, "error"]);
        return;
      }

      const result = await API.createItem({
        name: this.name,
        note: this.note,
        imageUrl: this.imageUrl,
        category: {
          id: this.categoryId,
        },
      });

      if (result instanceof Error)
        this.$emit("show-notification", [result.message, "error"]);
      else {
        this.name = "";
        this.note = "";
        this.imageUrl = "";
        this.categoryId = -1;

        this.$emit("close-reload");
        this.$emit("show-notification", ["Item successfully added", "success"]);
      }
    },
    async addCategory() {
      if (this.newCategoryName == null || this.newCategoryName === "") {
        this.$emit("show-notification", ["Category name is required", "error"]);
        return;
      }

      const result = await API.createCategory(this.newCategoryName);

      if (result instanceof Error) {
        this.$emit("show-notification", [result.message, "error"]);
        return;
      }
      this.$emit("reload-categories");
      this.$emit("show-notification", ["Category added", "success"]);
      this.newCategoryName = "";
    },
    validateFields() {
      if (this.name == null || this.name == "")
        return new Error("Name is required");
      if (this.categoryId == null || this.categoryId <= 0)
        return new Error("Invalid category");
    },
  },
};
</script>

<style scoped>
.add-item-container {
  padding: 4rem;
  position: relative;
}

.add-item-container h2 {
  font-weight: 500;
  font-size: 2.4rem;
  margin-bottom: 3rem;
}

.add-item-container .input-group {
  margin-bottom: 2rem;
}

.add-item-container .input-group:focus-within label {
  color: #f9a109;
}

.add-item-container .input-group:focus-within input,
.add-item-container .input-group:focus-within textarea,
.add-item-container .input-group:focus-within select {
  border-color: #f9a109;
}

.add-item-container .input-group label {
  display: block;
  font-weight: 500;
  font-size: 1.4rem;

  color: #34333a;
  margin-bottom: 0.5rem;
}

.add-item-container .input-group input,
.add-item-container .input-group textarea,
.add-item-container .input-group select {
  border-radius: 12px;
  padding: 1.5rem;
  border: 2px solid #bdbdbd;
  width: 100%;
  resize: none;
}

.add-item-container .input-group select option {
  font-weight: 500;
  color: #828282;
  font-size: 1.6rem;
  padding: 1rem 2rem;
  background: #f2f2f2;
}

.add-item-container .buttons-container {
  display: flex;
  column-gap: 1rem;
  align-items: center;
  justify-content: center;
}

.add-item-container .buttons-container button {
  border: none;
  padding: 2rem;
  cursor: pointer;
  border-radius: 12px;
  background: transparent;
}

.add-item-container .buttons-container button.save {
  color: #fff;
  background: #f9a109;
}

footer {
  position: absolute;
  left: 0;
  right: 0;
  bottom: 0;
  padding: 0 2rem 2rem;
}

footer button {
  width: 100%;
  padding: 1rem 1.5rem 1.5rem;
  font-size: 1.7rem;
  font-weight: bold;
  border: none;
  border-radius: 2rem;
  color: #fff;
  background: #f9a109;
  cursor: pointer;
}

footer input {
  width: 100%;
  margin-bottom: 1rem;
  border: 1px solid #c9c9c9;
  border-radius: 1rem;
  background: transparent;
  font-size: 1.5rem;
  padding: 1rem;
}
</style>
