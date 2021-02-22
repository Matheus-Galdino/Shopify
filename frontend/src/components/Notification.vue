<template>
  <div id="notification-container" :style="{ background: background }">    
    <main>
      <span class="material-icons"> {{ icon }} </span>          
      <p>{{ content }}</p>
    </main>
  </div>
</template>

<script>
export default {
  name: "Notification",
  props: {
    type: {
      type: String,      
    },
    content: {
      type: String,
      required: true,
    },
  },  
  computed: {
    background() {
      return this.type === "error" ? "#FF4737" : "#5bff8f";
    },
    icon() {
      return this.type === "error" ? "error" : "done";
    },
  },
};
</script>

<style scoped>
#notification-container {
  right: 30px;
  bottom: 20px;
  position: fixed;
  border-radius: 0.5rem;
  padding: 1.5rem;
}

main {
  display: flex;
  align-items: center;
  column-gap: 1rem;
}

p {
  color: #fff;
  font-size: 2rem;
}

span {
  font-size: 2.5rem;
  color: #fff;
}
</style>
