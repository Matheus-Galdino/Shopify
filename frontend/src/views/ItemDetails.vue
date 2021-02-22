<template>
  <div class="item-details">
    <header>
      <button @click="$emit('close')">
        <span class="material-icons"> keyboard_backspace </span>
        back
      </button>
    </header>

    <main>
      <img :src="item.imageUrl" v-if="item.imageUrl" />
      <img src="../assets/meal.svg" v-else />

      <section>
        <small>name</small>
        <h2>{{ item.name }}</h2>
      </section>

      <section>
        <small>category</small>
        <p>{{ item.category.name }}</p>
      </section>

      <section>
        <small>note</small>
        <p>{{ item.note }}</p>
      </section>
    </main>

    <footer>
      <button @click.prevent="deleteItem">delete</button>
      <button @click="$emit('add-to-list', item)" class="add-button">Add to list</button>
    </footer>
  </div>
</template>

<script>
import API from "@/API";

export default {
  name: "ItemDetails",
  props: {
    item: {
      type: Object,
      required: true,
    },
  },
  methods: {
    async deleteItem() {
      await API.deleteItem(this.item.id);
      this.$emit('show-notification', ['Item deleted', 'success'])
      this.$emit("reload");
    },

  },  
};
</script>

<style scoped>
.item-details {
  position: relative;
  background: #fff;
  padding: 4rem;
}

header {
  margin-bottom: 4rem;
}

header button {
  display: flex;
  column-gap: 5px;
  align-items: center;

  border: none;
  color: #f9a109;
  background: transparent;

  cursor: pointer;
}

main img {
  width: 30rem;
  height: 22rem;
  object-fit: cover;
  border-radius: 25px;

  margin-bottom: 5rem;
}

main section {
  margin-bottom: 3rem;
}

main small {
  display: block;
  font-weight: 500;
  font-size: 1.2rem;

  color: #c1c1c4;
  margin-bottom: 1rem;
}

main h2 {
  font-weight: 500;
  font-size: 2.4rem;
}

main p {
  font-weight: 500;
  font-size: 1.8rem;
}

footer {
  display: flex;
  column-gap: 3rem;
  justify-content: center;

  position: absolute;
  left: 0;
  right: 0;
  bottom: 30px;
}

footer button {
  font-weight: bold;
  font-size: 1.6rem;

  padding: 2rem;

  border: none;
  cursor: pointer;
  color: #34333a;
  background: transparent;
}

footer .add-button {
  color: #ffffff;
  border-radius: 12px;
  background: #f9a109;
}
</style>
